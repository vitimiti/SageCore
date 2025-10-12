// -----------------------------------------------------------------------
// <copyright file="Buffer.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Compression.Lzhl;

internal abstract class Buffer
{
    protected byte[] Data { get; } = new byte[Constants.BufferSize];

    protected uint Position { get; set; }

    protected Buffer() { }

    protected static int Wrap(uint position) => (int)(position & Constants.BufferMask);

    protected static int Distance(int difference) => difference & Constants.BufferMask;

    protected void ToBuffer(byte c) => Data[Wrap(Position++)] = c;

    protected void ToBuffer(ReadOnlySpan<byte> source)
    {
        if (source.Length >= Constants.BufferSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(source),
                $"Source length must be less than buffer {Constants.BufferSize} bytes."
            );
        }

        var begin = Wrap(Position);
        var end = begin + source.Length;
        if (end > Constants.BufferSize)
        {
            var left = Constants.BufferSize - begin;
            source[..left].CopyTo(Data.AsSpan(begin, left));
            var right = source.Length - left;
            source[left..right].CopyTo(Data.AsSpan(0, right));
        }
        else
        {
            source.CopyTo(Data.AsSpan(begin, source.Length));
        }

        Position += (uint)source.Length;
    }

    protected void BufferCopy(ReadOnlySpan<byte> source, int position)
    {
        if (source.Length >= Constants.BufferSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(source),
                $"Source length must be less than buffer {Constants.BufferSize} bytes."
            );
        }

        var begin = Wrap((uint)position);
        var end = begin + source.Length;
        if (end > Constants.BufferSize)
        {
            var left = Constants.BufferSize - begin;
            source[..left].CopyTo(Data.AsSpan(begin, left));
            var right = source.Length - left;
            source[left..right].CopyTo(Data.AsSpan(0, right));
        }
        else
        {
            source.CopyTo(Data.AsSpan(begin, source.Length));
        }
    }

    protected int MatchCount(int wrappedPosition, ReadOnlySpan<byte> source, int limitCount)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(limitCount, Constants.BufferSize);

        var begin = wrappedPosition;
        if (Constants.BufferSize - begin >= limitCount)
        {
            for (var i = 0; i < limitCount; i++)
            {
                if (Data[begin + i] != source[i])
                {
                    return i;
                }
            }

            return limitCount;
        }

        for (var i = begin; i < Constants.BufferSize; i++)
        {
            if (Data[i] != source[i - begin])
            {
                return i - begin;
            }
        }

        var shift = Constants.BufferSize - begin;
        var count = limitCount - shift;
        for (var i = 0; i < count; i++)
        {
            if (Data[i] != source[shift + i])
            {
                return shift + i;
            }
        }

        return limitCount;
    }
}
