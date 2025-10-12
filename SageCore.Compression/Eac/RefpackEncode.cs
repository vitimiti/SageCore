// -----------------------------------------------------------------------
// <copyright file="RefpackEncode.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Utilities;

namespace SageCore.Compression.Eac;

internal static partial class Refpack
{
    /// <summary>
    /// Public entry point: Encode source into compressedData. Returns number of bytes written.
    /// </summary>
    /// <param name="compressedData">The span to write the compressed data to.</param>
    /// <param name="source">The span of uncompressed data to encode.</param>
    /// <returns>The number of bytes written to the compressedData span.</returns>
    public static int Encode(Span<byte> compressedData, ReadOnlySpan<byte> source)
    {
        // simple fb6 header
        int headerLength;
        if (source.Length > 0xFFFFFF)
        {
            // 0x90fb then 4-byte size
            compressedData.WriteUInt16BigEndian(0x90FB);
            compressedData[2..].WriteUInt32BigEndian((uint)source.Length);
            headerLength = 6;
        }
        else
        {
            // 0x10fb then 3-byte size
            compressedData.WriteUInt16BigEndian(0x10FB);
            compressedData[2..].WriteUInt24BigEndian((uint)source.Length);
            headerLength = 5;
        }

        var compressedBodyLength = Compress(source, compressedData[headerLength..]);
        return headerLength + compressedBodyLength;
    }

    private static int MatchLength(ReadOnlySpan<byte> data, int aIndex, int bIndex, int maxMatch)
    {
        var current = 0;
        var availableA = data.Length - aIndex;
        var availableB = data.Length - bIndex;
        var limit = Math.Min(maxMatch, Math.Min(availableA, availableB));
        while (current < limit && data[aIndex + current] == data[bIndex + current])
        {
            ++current;
        }

        return current;
    }

    private static int Hash(ReadOnlySpan<byte> data, int index) =>
        (int)((((uint)data[index] << 8) | data[index + 2]) ^ ((uint)data[index + 1] << 4));

    // Core compression routine ported from refcompress. Returns number of bytes written to dest.
    private static int Compress(ReadOnlySpan<byte> source, Span<byte> dest)
    {
        var hashtbl = new int[65536];
        var link = new int[131072];
        for (var i = 0; i < hashtbl.Length; ++i)
        {
            hashtbl[i] = -1;
        }

        var toIndex = 0; // write pointer into dest
        var run = 0;
        var cptr = 0; // current read pointer in source
        var rptr = 0; // start of current literal run

        var len = source.Length;
        if (len < 0)
        {
            len = 0;
        }

        len -= 4;

        while (len >= 0)
        {
            var boffset = 0;
            var blen = 2;
            var bcost = 2;
            var mlen = Math.Min(len, 1028);

            var hash = Hash(source, cptr);
            var hoffset = hashtbl[hash];
            var minHoffset = Math.Max(cptr - 131071, 0);

            if (hoffset >= minHoffset)
            {
                var probe = hoffset;

                do
                {
                    var tptr = probe; // candidate match position

                    // quick check: ensure index for comparing blen exists
                    if (
                        cptr + blen < source.Length
                        && tptr + blen < source.Length
                        && source[cptr + blen] == source[tptr + blen]
                    )
                    {
                        var tlen = MatchLength(source, cptr, tptr, mlen);

                        if (tlen > blen)
                        {
                            var toffset = cptr - 1 - tptr;
                            var tcost = 4;

                            if (toffset < 1024 && tlen <= 10)
                            {
                                tcost = 2;
                            }
                            else if (toffset < 16384 && tlen <= 67)
                            {
                                tcost = 3;
                            }

                            if (tlen - tcost + 4 > blen - bcost + 4)
                            {
                                blen = tlen;
                                bcost = tcost;
                                boffset = toffset;

                                if (blen >= 1028)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    probe = link[probe & 131071];
                } while (probe >= minHoffset);
            }

            if (bcost >= blen || len < 4)
            {
                // no useful match found; insert current position into hash chain and advance
                var hoff = cptr;
                link[hoff & 131071] = hashtbl[hash];
                hashtbl[hash] = hoff;
                ++run;
                ++cptr;
                --len;
            }
            else
            {
                // flush any pending literal runs longer than 3
                while (run > 3)
                {
                    var tlen = Math.Min(112, run & ~3);

                    run -= tlen;
                    dest[toIndex++] = (byte)(0xE0 + (tlen >> 2) - 1);
                    source.Slice(rptr, tlen).CopyTo(dest.Slice(toIndex, tlen));
                    rptr += tlen;
                    toIndex += tlen;
                }

                // emit reference according to chosen bcost
                if (bcost == 2)
                {
                    dest[toIndex++] = (byte)(((boffset >> 8) << 5) + ((blen - 3) << 2) + run);
                    dest[toIndex++] = (byte)boffset;
                }
                else if (bcost == 3)
                {
                    dest[toIndex++] = (byte)(0x80 + (blen - 4));
                    dest[toIndex++] = (byte)((run << 6) + (boffset >> 8));
                    dest[toIndex++] = (byte)boffset;
                }
                else
                {
                    // bcost == 4
                    dest[toIndex++] = (byte)(0xC0 + ((boffset >> 16) << 4) + (((blen - 5) >> 8) << 2) + run);
                    dest[toIndex++] = (byte)(boffset >> 8);
                    dest[toIndex++] = (byte)boffset;
                    dest[toIndex++] = (byte)(blen - 5);
                }

                if (run != 0)
                {
                    source.Slice(rptr, run).CopyTo(dest.Slice(toIndex, run));
                    toIndex += run;
                    run = 0;
                }

                // update hash entries for every position within the matched block
                for (var i = 0; i < blen; ++i)
                {
                    var h = Hash(source, cptr);
                    var hoff = cptr;
                    link[hoff & 131071] = hashtbl[h];
                    hashtbl[h] = hoff;
                    ++cptr;
                }

                rptr = cptr;
                len -= blen;
            }
        }

        len += 4;
        run += len;
        while (run > 3)
        {
            var tlen = Math.Min(112, run & ~3);

            run -= tlen;
            dest[toIndex++] = (byte)(0xE0 + (tlen >> 2) - 1);
            source.Slice(rptr, tlen).CopyTo(dest.Slice(toIndex, tlen));
            rptr += tlen;
            toIndex += tlen;
        }

        dest[toIndex++] = (byte)(0xFC + run);
        if (run != 0)
        {
            source.Slice(rptr, run).CopyTo(dest[toIndex..]);
            toIndex += run;
        }

        return toIndex;
    }
}
