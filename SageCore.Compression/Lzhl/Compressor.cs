// -----------------------------------------------------------------------
// <copyright file="Compressor.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Compression.Lzhl;

internal sealed class Compressor : Buffer
{
    private readonly EncoderStat _stat = new();
    private readonly ushort[] _table = new ushort[Constants.TableSize];

    public Compressor()
        : base() => Array.Fill(_table, unchecked((ushort)-1));

    public static int CalculateMaximumBufferSize(int rawSize) => Encoder.CalculateMaximumBufferSize(rawSize);

    /// <summary>
    /// Compresses the specified source data into the destination buffer.
    /// </summary>
    /// <param name="destination">The destination buffer to hold compressed data.</param>
    /// <param name="source">The source buffer containing data to be compressed.</param>
    /// <returns>The size of the compressed data written to the destination buffer.</returns>
    public long Compress(Span<byte> destination, ReadOnlySpan<byte> source)
    {
        Encoder coder = new(_stat, destination.ToArray());

        var sourceIndex = 0;
        var sourceLength = source.Length;
        var sourceEnd = sourceLength;

        var hash = 0U;
        if (sourceLength >= Constants.Match)
        {
            for (var i = 0; i < Constants.Match; i++)
            {
                hash = UpdateHash(hash, source[i]);
            }
        }

        while (true)
        {
            var sourceLeft = sourceEnd - sourceIndex;
            if (sourceLeft < Constants.Match)
            {
                if (sourceLeft > 0)
                {
                    ToBuffer(source.Slice(sourceIndex, sourceLeft));
                    coder.PutRaw(source[sourceIndex..sourceLeft]);
                }

                break;
            }

            var rawCount = 0;
            var maxRaw = int.Min(sourceLeft - Constants.Match, Encoder.MaxRaw);
            while (true)
            {
                var hashInner = HashPosition(hash);
                var hashPosition = _table[hashInner];
                var wrappedBufferPosition = Wrap(Position);
                _table[hashInner] = (ushort)wrappedBufferPosition;

                var matchLength = 0;
                if (hashPosition != unchecked((ushort)-1) && hashPosition != wrappedBufferPosition)
                {
                    var matchLimit = int.Min(
                        int.Min(Distance(wrappedBufferPosition - hashPosition), sourceLeft - rawCount),
                        Constants.Match + Encoder.MaxMatchOver
                    );

                    matchLength = MatchCount(hashPosition, source[(sourceIndex + rawCount)..], matchLimit);
                }

                if (matchLength >= Constants.Min)
                {
                    coder.PutMatch(
                        source[(sourceIndex + rawCount)..].ToArray(),
                        matchLength - Constants.Min,
                        Distance(wrappedBufferPosition - hashPosition)
                    );

                    var availableAfter = sourceLeft - rawCount - 1;
                    var updateLength = int.Min(matchLength - 1, Math.Max(0, availableAfter));
                    hash = UpdateTable(hash, source.Slice(sourceIndex + rawCount + 1, updateLength), Position + 1u);

                    ToBuffer(source.Slice(sourceIndex + rawCount, matchLength));
                    sourceIndex += rawCount + matchLength;
                    break;
                }

                if (rawCount + 1 > maxRaw)
                {
                    if (rawCount + Constants.Match >= sourceLeft && sourceLeft <= Encoder.MaxRaw)
                    {
                        ToBuffer(source.Slice(sourceIndex + rawCount, sourceLeft - rawCount));
                        rawCount = sourceLeft;
                    }

                    coder.PutRaw(source.Slice(sourceIndex, rawCount));
                    sourceIndex += rawCount;
                    break;
                }

                hash = UpdateHash(hash, source[sourceIndex + rawCount]);
                ToBuffer(source[rawCount++]);
            }
        }

        return coder.Flush();
    }

    private static uint UpdateHash(uint hash, byte value) => (hash << Constants.HashShift) ^ value;

    private static uint UpdateHash(uint hash, ReadOnlySpan<byte> source) =>
        (hash << Constants.HashShift) ^ source[Constants.Match];

    private static int HashPosition(uint hash) => (int)(hash & Constants.HashMask);

    private uint UpdateTable(uint hash, ReadOnlySpan<byte> source, uint position)
    {
        if (source.Length <= 0)
        {
            return 0;
        }

        var sourcePosition = 0;
        if (source.Length > Constants.SkipHash)
        {
            sourcePosition++;
            hash = 0;
            var end = sourcePosition + source.Length - Constants.Match;
            for (; sourcePosition < end; sourcePosition++)
            {
                hash = UpdateHash(hash, source[sourcePosition]);
            }

            return hash;
        }

        hash = UpdateHash(hash, source);
        sourcePosition++;

        for (var i = 0; i < source.Length; i++)
        {
            _table[HashPosition(hash)] = (ushort)Wrap((uint)(position + i));
            hash = UpdateHash(hash, source[sourcePosition + i]);
        }

        return hash;
    }
}
