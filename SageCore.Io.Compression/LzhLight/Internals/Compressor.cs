// -----------------------------------------------------------------------
// <copyright file="Compressor.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal class Compressor : Buffer
{
    private const int HashShift = (Constants.LzTableBits + Constants.LzMatch - 1) / Constants.LzMatch;

    private readonly EncoderStat _stat = new();
    private readonly ushort[] _table = new ushort[Constants.LzTableBits];

    public Compressor()
    {
        Array.Fill(_table, unchecked((ushort)-1));
    }

    public static int CalcMaxBuffer(int rawSize) => Encoder.CalcMaxBuffer(rawSize);

    public long Compress(Span<byte> destination, ReadOnlySpan<byte> source)
    {
        Encoder coder = new(_stat, destination);
        var srcIndex = 0;
        var srcEnd = source.Length;

        uint hash = 0;
        if (source.Length >= Constants.LzMatch)
        {
            const int pEnd = Constants.LzMatch;
            for (var p = 0; p < pEnd; p++)
            {
                hash = UpdateHash(hash, source[p]);
            }
        }

        while (true)
        {
            var srcLeft = srcEnd - srcIndex;
            if (srcLeft < Constants.LzMatch)
            {
                if (srcLeft > 0)
                {
                    ToBuf(source.Slice(srcIndex, srcLeft));
                    coder.PutRaw(source.Slice(srcIndex, srcLeft));
                }

                break;
            }

            var nRaw = 0;
            var maxRaw = int.Min(srcLeft - Constants.LzMatch, Encoder.MaxRaw);

            while (true)
            {
                var hash2 = HashPos(hash);

                var hashPos = _table[hash2];
                var wrapBufPos = Wrap(BufPos);
                _table[hash2] = (ushort)wrapBufPos;

                var matchLen = 0;
                if (hashPos != unchecked((ushort)-1) && hashPos != wrapBufPos)
                {
                    var matchLimit = int.Min(
                        int.Min(Distance(wrapBufPos - hashPos), srcLeft - nRaw),
                        Constants.LzMin + Encoder.MaxMatchOver
                    );

                    matchLen = MatchCount(hashPos, source[(srcIndex + nRaw)..], matchLimit);
                }

                if (matchLen >= Constants.LzMin)
                {
                    coder.PutMatch(
                        source.Slice(srcIndex, nRaw),
                        matchLen - Constants.LzMin,
                        Distance(wrapBufPos - hashPos)
                    );

                    ReadOnlySpan<byte> updateSrc = source[(srcIndex + nRaw + 1)..];
                    var updateLen = int.Min(matchLen - 1, srcEnd - (srcIndex + nRaw + 1));
                    if (updateLen > 0 && updateSrc.Length >= Constants.LzMatch + 1)
                    {
                        var tempIndex = 0;
                        hash = UpdateTable(hash, updateSrc[..updateLen], ref tempIndex, BufPos + 2);
                    }

                    ToBuf(source.Slice(srcIndex + nRaw, matchLen));
                    srcIndex += nRaw + matchLen;
                    break;
                }

                if (nRaw + 1 > maxRaw)
                {
                    if (nRaw + Constants.LzMatch >= srcLeft && srcLeft <= Encoder.MaxRaw)
                    {
                        ToBuf(source.Slice(srcIndex + nRaw, srcLeft - nRaw));
                        nRaw = srcLeft;
                    }

                    coder.PutRaw(source.Slice(srcIndex, nRaw));
                    srcIndex += nRaw;
                    break;
                }

                ReadOnlySpan<byte> hashUpdateSrc = source[(srcIndex + nRaw)..];
                if (hashUpdateSrc.Length >= Constants.LzMatch + 1)
                {
                    hash = UpdateHash(hash, hashUpdateSrc);
                }
                else if (hashUpdateSrc.Length > 0)
                {
                    hash = UpdateHash(hash, hashUpdateSrc[0]);
                }

                ToBuf(source[srcIndex + nRaw]);
                nRaw++;
            }
        }

        return coder.Flush();
    }

    private static uint UpdateHash(uint hash, byte c) => (hash << HashShift) ^ c;

    private static uint UpdateHash(uint hash, ReadOnlySpan<byte> src) =>
        src.Length < Constants.LzMatch + 1
            ? throw new ArgumentOutOfRangeException(
                nameof(src),
                $"The length of the span must be greater than {Constants.LzMatch + 1}."
            )
            : (hash << HashShift) ^ src[Constants.LzMatch];

    private static uint HashPos(uint hash) => ((hash * 0x03_43_FD) + 0x26_9E_C3) >> (32 - Constants.LzTableBits);

    private uint UpdateTable(uint hash, ReadOnlySpan<byte> src, ref int srcIndex, int pos)
    {
        if (src.Length <= 0)
        {
            return 0;
        }

        if (src.Length > Constants.LzSkipHash)
        {
            ++srcIndex;
            hash = 0;
            var pEnd = srcIndex + src.Length + Constants.LzSkipHash;
            for (var p = srcIndex + src.Length; p < pEnd; ++p)
            {
                hash = UpdateHash(hash, src[p]);
            }

            return hash;
        }

        hash = UpdateHash(hash, src);
        ++srcIndex;

        for (var i = 0; i < src.Length; i++)
        {
            _table[HashPos(hash)] = (ushort)Wrap(pos + i);
            hash = UpdateHash(hash, src[(srcIndex + i)..]);
        }

        return hash;
    }
}
