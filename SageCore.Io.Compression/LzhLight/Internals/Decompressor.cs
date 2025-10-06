// -----------------------------------------------------------------------
// <copyright file="Decompressor.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal class Decompressor : Buffer
{
    private readonly DecoderStat _stat = new();
    private readonly HuffStatTemp[] _temps = new HuffStatTemp[Constants.HuffSymbolsCount];
    private uint _bits;
    private int _bitsCount;

    public bool Decompress(
        Span<byte> destination,
        ref int destinationBytesWritten,
        ReadOnlySpan<byte> source,
        ref int sourceBytesRead
    )
    {
        var destIndex = 0;
        var srcIndex = 0;
        var endDest = destination.Length;
        _bitsCount = 0;
        _bits = 0;

        while (true)
        {
            var grp = Get(source, ref srcIndex, 4);
            if (grp < 0)
            {
                return false;
            }

            ref DecoderStat.Group group = ref _stat.GroupTable[grp];

            int symbol;
            var bitsCount = group.BitsCount;
            if (bitsCount == 0)
            {
                symbol = _stat.SymbolTable[group.Pos];
            }
            else
            {
                var got = Get(source, ref srcIndex, bitsCount);
                if (got < 0)
                {
                    return false;
                }

                var pos = group.Pos + got;
                if (pos >= Constants.HuffSymbolsCount)
                {
                    return false;
                }

                symbol = _stat.SymbolTable[pos];
            }

            _stat.Stat[symbol]++;

            if (symbol < 256)
            {
                if (destIndex >= endDest)
                {
                    return false;
                }

                destination[destIndex++] = (byte)symbol;
                ToBuf((byte)symbol);
                continue;
            }
            else if (symbol == Constants.HuffSymbolsCount - 2)
            {
                // Rebuild Huffman table
                _ = _stat.MakeSortedTemp(_temps);

                for (var i = 0; i < Constants.HuffSymbolsCount; i++)
                {
                    _stat.SymbolTable[i] = _temps[i].I;
                }

                var lastBitsCount = 0;
                var pos = 0;
                for (var i = 0; i < 16; i++)
                {
                    var n = 0;
                    while (true)
                    {
                        var bit = Get(source, ref srcIndex, 1);
                        if (bit == 1)
                        {
                            break;
                        }

                        if (bit < 0)
                        {
                            return false;
                        }

                        n++;
                    }

                    lastBitsCount += n;

                    _stat.GroupTable[i].BitsCount = lastBitsCount;
                    _stat.GroupTable[i].Pos = pos;
                    pos += 1 << lastBitsCount;
                }

                continue;
            }
            else if (symbol == Constants.HuffSymbolsCount - 1)
            {
                break;
            }

            // Handle match length
            int matchOver;
            if (symbol < 256 + 8)
            {
                matchOver = symbol - 256;
            }
            else
            {
                var matchOverTableIndex = symbol - 256 - 8;
                (var extraBitsCount, var baseValue) = GetMatchOverItem(matchOverTableIndex);
                var extra = Get(source, ref srcIndex, extraBitsCount);
                if (extra < 0)
                {
                    return false;
                }

                matchOver = baseValue + extra;
            }

            // Handle displacement
            var dispPrefix = Get(source, ref srcIndex, 3);
            if (dispPrefix < 0)
            {
                return false;
            }

            (var dispBitsCount, var dispBase) = GetDispItem(dispPrefix);
            bitsCount = dispBitsCount + Constants.LzBufBits - 7;

            var disp = 0;
            if (bitsCount > 8)
            {
                bitsCount -= 8;
                var highBits = Get(source, ref srcIndex, 8);
                if (highBits < 0)
                {
                    return false;
                }

                disp |= highBits << bitsCount;
            }

            var got2 = Get(source, ref srcIndex, bitsCount);
            if (got2 < 0)
            {
                return false;
            }

            disp |= got2;
            disp += dispBase << (Constants.LzBufBits - 7);

            var matchLen = matchOver + Constants.LzMin;
            if (destIndex + matchLen > endDest)
            {
                return false;
            }

            var pos2 = BufPos - disp;
            Span<byte> destSpan = destination.Slice(destIndex, matchLen);

            if (matchLen < disp)
            {
                BufCpy(destSpan, pos2);
            }
            else
            {
                BufCpy(destSpan[..disp], pos2);
                for (var i = 0; i < matchLen - disp; i++)
                {
                    destSpan[i + disp] = destSpan[i];
                }
            }

            ToBuf(destSpan);
            destIndex += matchLen;
        }

        destinationBytesWritten = destIndex;
        sourceBytesRead = srcIndex;
        return true;
    }

    private static (int ExtraBitsCount, int BaseValue) GetMatchOverItem(int index) =>
        index switch
        {
            0 => (1, 8),
            1 => (2, 10),
            2 => (3, 14),
            3 => (4, 22),
            4 => (5, 38),
            5 => (6, 70),
            6 => (7, 134),
            7 => (8, 262),
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Index must be in range 0-7."),
        };

    private static (int BitsCount, int Disp) GetDispItem(int prefix) =>
        prefix switch
        {
            0 => (0, 0),
            1 => (0, 1),
            2 => (1, 2),
            3 => (2, 4),
            4 => (3, 8),
            5 => (4, 16),
            6 => (5, 32),
            7 => (6, 64),
            _ => throw new ArgumentOutOfRangeException(nameof(prefix), "Prefix must be in range 0-7."),
        };

    private int Get(ReadOnlySpan<byte> source, ref int srcIndex, int n)
    {
        if (_bitsCount < n)
        {
            if (srcIndex >= source.Length)
            {
                _bitsCount = 0;
                return -1;
            }

            _bits |= (uint)(source[srcIndex++] << (24 - _bitsCount));
            _bitsCount += 8;
        }

        var ret = (int)(_bits >> (32 - n));
        _bits <<= n;
        _bitsCount -= n;
        return ret;
    }
}
