// -----------------------------------------------------------------------
// <copyright file="Decompressor.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
using DispItem = (int BitsCount, int Disp);
using MatchOverItem = (int ExtraBitsCount, int Base);
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly

namespace SageCore.Compression.Lzhl;

internal sealed class Decompressor : Buffer
{
    private static readonly MatchOverItem[] MatchOverTable =
    [
        (1, 8),
        (2, 10),
        (3, 14),
        (4, 22),
        (5, 38),
        (6, 70),
        (7, 134),
        (8, 262),
    ];

    private static readonly DispItem[] DispTable = [(0, 0), (0, 1), (1, 2), (2, 4), (3, 8), (4, 16), (5, 32), (6, 64)];

    private readonly DecoderStat _stat = new();

    private uint _bits;
    private int _bitsCount;

    /// <summary>
    /// Decompresses the specified source data into the destination buffer.
    /// </summary>
    /// <param name="destination">The destination buffer to hold decompressed data.</param>
    /// <param name="source">The source buffer containing compressed data.</param>
    /// <returns>True if decompression is successful; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when destination or source is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when destinationSize is negative.</exception>
    /// <exception cref="InvalidOperationException">Thrown when an internal invariant is violated during decompression.</exception>
    public bool TryDecompress(Span<byte> destination, ReadOnlySpan<byte> source)
    {
        var sourceEnd = source.Length;
        var destinationEnd = destination.Length;

        _bits = 0;
        _bitsCount = 0;

        var srcIndex = 0;
        var dstIndex = 0;
        while (true)
        {
            var got = Get(source, ref srcIndex, sourceEnd, 4);
            if (got < 0)
            {
                return false;
            }

            DecoderStat.Group group = _stat.GroupTable[got];

            int symbol;
            var bitsCount = group.BitsCount;
            if (bitsCount > 8)
            {
                throw new InvalidOperationException($"Group bits count {bitsCount} is greater than 8.");
            }

            if (bitsCount == 0)
            {
                symbol = _stat.SymbolTable[group.Position];
            }
            else
            {
                var gotInner = Get(source, ref srcIndex, sourceEnd, bitsCount);
                if (gotInner < 0)
                {
                    return false;
                }

                var pos = group.Position + gotInner;
                if (pos >= Constants.HuffSymbolsCount)
                {
                    return false;
                }

                symbol = _stat.SymbolTable[pos];
            }

            if (symbol >= Constants.HuffSymbolsCount)
            {
                throw new InvalidOperationException($"Decoded symbol {symbol} is out of range.");
            }

            _stat.Stat[symbol]++;

            if (symbol < 256)
            {
                if (dstIndex >= destinationEnd)
                {
                    return false;
                }

                destination[dstIndex++] = (byte)symbol;
                ToBuffer((byte)symbol);
                continue;
            }
            else if (symbol == Constants.HuffSymbolsCount - 2)
            {
                var temp = new HuffTempStat[Constants.HuffSymbolsCount];
                _ = _stat.MakeSortedTempStats(temp.AsSpan());
                for (var i = 0; i < Constants.HuffSymbolsCount; i++)
                {
                    _stat.SymbolTable[i] = temp[i].I;
                }

                var lastBitsCount = 0;
                var position = 0;
                for (var i = 0; i < 16; ++i)
                {
                    var count = 0;
                    while (true)
                    {
                        var bit = Get(source, ref srcIndex, sourceEnd, 1);
                        if (bit < 0)
                        {
                            return false;
                        }

                        if (bit != 0)
                        {
                            break;
                        }

                        ++count;
                    }

                    lastBitsCount += count;
                    _stat.GroupTable[i].BitsCount = lastBitsCount;
                    _stat.GroupTable[i].Position = position;
                    position += 1 << lastBitsCount;
                }

                if (position >= Constants.HuffSymbolsCount + 255)
                {
                    throw new InvalidOperationException($"Rebuilt symbol table position {position} is too large.");
                }

                continue;
            }
            else if (symbol == Constants.HuffSymbolsCount - 1)
            {
                break;
            }

            int matchOver;
            if (symbol < 256 + 8)
            {
                matchOver = symbol - 256;
            }
            else
            {
                (var extraBitsCount, var @base) = MatchOverTable[symbol - 256 - 8];
                var extra = Get(source, ref srcIndex, sourceEnd, extraBitsCount);
                if (extra < 0)
                {
                    return false;
                }

                matchOver = @base + extra;
            }

            var dispPrefix = Get(source, ref srcIndex, sourceEnd, 3);
            if (dispPrefix < 0)
            {
                return false;
            }

            (var bitsCount2, var dispInner) = DispTable[dispPrefix];
            bitsCount2 = bitsCount2 + Constants.BufferBits - 7;
            if (bitsCount2 > 16)
            {
                throw new InvalidOperationException($"Displacement bits count {bitsCount2} is greater than 16.");
            }

            var disp = 0;
            if (bitsCount2 > 8)
            {
                bitsCount2 -= 8;
                var part = Get(source, ref srcIndex, sourceEnd, 8);
                if (part < 0)
                {
                    return false;
                }

                disp |= part << bitsCount2;
            }

            var got2 = Get(source, ref srcIndex, sourceEnd, bitsCount2);
            if (got2 < 0)
            {
                return false;
            }

            disp |= got2;
            disp += dispInner << (Constants.BufferBits - 7);

            if (disp is < 0 or >= Constants.BufferSize)
            {
                // original code asserted disp in range; this is an internal invariant -> throw
                throw new InvalidOperationException(
                    $"Displacement {disp} is out of range (0..{Constants.BufferSize - 1})."
                );
            }

            var matchLength = matchOver + Constants.Min;
            if (dstIndex + matchLength > destinationEnd)
            {
                return false;
            }

            var bufferStart = Wrap(Position - (uint)disp);
            if (matchLength < disp)
            {
                var begin = bufferStart;
                if (begin + matchLength <= Constants.BufferSize)
                {
                    new ReadOnlySpan<byte>(Data, begin, matchLength).CopyTo(destination.Slice(dstIndex, matchLength));
                }
                else
                {
                    var left = Constants.BufferSize - begin;
                    new ReadOnlySpan<byte>(Data, begin, left).CopyTo(destination.Slice(dstIndex, left));
                    new ReadOnlySpan<byte>(Data, 0, matchLength - left).CopyTo(
                        destination.Slice(dstIndex + left, matchLength - left)
                    );
                }
            }
            else
            {
                if (disp > 0)
                {
                    var begin = bufferStart;
                    if (begin + disp <= Constants.BufferSize)
                    {
                        new ReadOnlySpan<byte>(Data, begin, disp).CopyTo(destination.Slice(dstIndex, disp));
                    }
                    else
                    {
                        var left = Constants.BufferSize - begin;
                        new ReadOnlySpan<byte>(Data, begin, left).CopyTo(destination.Slice(dstIndex, left));
                        new ReadOnlySpan<byte>(Data, 0, disp - left).CopyTo(
                            destination.Slice(dstIndex + left, disp - left)
                        );
                    }
                }

                for (var i = 0; i < matchLength - disp; ++i)
                {
                    destination[dstIndex + disp + i] = destination[dstIndex + i];
                }
            }

            ToBuffer(destination.Slice(dstIndex, matchLength));
            dstIndex += matchLength;
        }

        return true;
    }

    private int Get(ReadOnlySpan<byte> src, ref int srcIndex, int srcEnd, int n)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, 8);

        if (_bitsCount < n)
        {
            if (srcIndex >= srcEnd)
            {
                _bitsCount = 0;
                return -1;
            }

            _bits |= (uint)(src[srcIndex++] << (24 - _bitsCount));
            _bitsCount += 8;
        }

        var ret = (int)(_bits >> (32 - n));
        _bits <<= n;
        _bitsCount -= n;
        return ret;
    }
}
