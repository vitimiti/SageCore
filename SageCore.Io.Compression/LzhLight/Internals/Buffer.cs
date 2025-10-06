// -----------------------------------------------------------------------
// <copyright file="Buffer.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal abstract class Buffer
{
    protected byte[] Buf { get; } = new byte[Constants.LzBufSize];

    protected int BufPos { get; set; }

    protected static int Wrap(int pos) => pos & Constants.LzBufMask;

    protected static int Distance(int diff) => diff & Constants.LzBufMask;

    protected void ToBuf(byte c) => Buf[Wrap(BufPos++)] = c;

    protected void ToBuf(ReadOnlySpan<byte> src)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(src.Length, Constants.LzBufSize);
        var begin = Wrap(BufPos);
        var end = begin + src.Length;
        if (end > Constants.LzBufSize)
        {
            var left = Constants.LzBufSize - begin;
            Array.Copy(src.ToArray(), 0, Buf, begin, left);
            Array.Copy(src.ToArray(), left, Buf, 0, src.Length - left);
        }
        else
        {
            src.CopyTo(Buf.AsSpan(begin, src.Length));
        }

        BufPos += src.Length;
    }

    protected void BufCpy(Span<byte> dest, int pos)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(dest.Length, Constants.LzBufSize);
        var begin = Wrap(pos);
        var end = begin + dest.Length;
        if (end > Constants.LzBufSize)
        {
            var left = Constants.LzBufSize - begin;
            Array.Copy(Buf, begin, dest.ToArray(), 0, left);
            Array.Copy(Buf, 0, dest.ToArray(), left, dest.Length - left);
        }
        else
        {
            Buf.AsSpan(begin, dest.Length).CopyTo(dest);
        }
    }

    protected int MatchCount(int wrappedPos, ReadOnlySpan<byte> src, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(limit, Constants.LzBufSize);
        var begin = wrappedPos;
        if (Constants.LzBufSize - begin >= limit)
        {
            for (var i = 0; i < limit; i++)
            {
                if (Buf[begin + i] != src[i])
                {
                    return i;
                }
            }

            return limit;
        }

        for (var i = 0; i < Constants.LzBufSize; i++)
        {
            if (Buf[i] != src[i - begin])
            {
                return i - begin;
            }
        }

        var shift = Constants.LzBufSize - begin;
        var n = limit - (Constants.LzBufSize - begin);
        for (var j = 0; j < n; j++)
        {
            if (Buf[j] != src[shift + j])
            {
                return shift + j;
            }
        }

        return limit;
    }
}
