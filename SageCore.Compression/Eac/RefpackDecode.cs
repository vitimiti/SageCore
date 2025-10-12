// -----------------------------------------------------------------------
// <copyright file="RefpackDecode.cs" company="SageCore Contributors">
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
    /// Decompresses Refpack compressed data into the provided destination span.
    /// </summary>
    /// <param name="destination">The span to write the decompressed data to.</param>
    /// <param name="compressedData">The Refpack compressed data to decompress.</param>
    /// <returns>The number of bytes written to the destination span.</returns>
    /// <exception cref="ArgumentException">Thrown if the compressed data is not a valid Refpack stream.</exception>
    /// <remarks>
    /// This method assumes that the destination span is large enough to hold the decompressed data.
    /// </remarks>
    public static int Decode(Span<byte> destination, ReadOnlySpan<byte> compressedData)
    {
        if (!IsValid(compressedData))
        {
            throw new ArgumentException("Data is not a valid Refpack compressed stream.", nameof(compressedData));
        }

        var destinationIndex = 0;
        var compressedDataIndex = 0;

        var type = (uint)compressedData[compressedDataIndex++];
        type = (type << 8) + compressedData[compressedDataIndex++];

        int unpackedLength;

        // 4-byte size field
        if ((type & 0x80_00) != 0)
        {
            // skip unpackedLength field
            if ((type & 0x01_00) != 0)
            {
                compressedDataIndex += 4;
            }

            unpackedLength = compressedData[compressedDataIndex++];
            unpackedLength = (unpackedLength << 8) + compressedData[compressedDataIndex++];
        }
        else
        {
            // skip unpackedLength field
            if ((type & 0x01_00) != 0)
            {
                compressedDataIndex += 3;
            }

            unpackedLength = compressedData[compressedDataIndex++];
        }

        unpackedLength = (unpackedLength << 8) + compressedData[compressedDataIndex++];
        unpackedLength = (unpackedLength << 8) + compressedData[compressedDataIndex++];

        if (unpackedLength != DecompressedSize(compressedData))
        {
            throw new ArgumentException("Data is not a valid Refpack compressed stream.", nameof(compressedData));
        }

        while (true)
        {
            var first = compressedData[compressedDataIndex++];
            int run;

            // short form
            if ((first & 0x80) == 0)
            {
                var second = compressedData[compressedDataIndex++];
                run = first & 3;
                while (run-- > 0)
                {
                    destination[destinationIndex++] = compressedData[compressedDataIndex++];
                }

                var referenceIndex = destinationIndex - 1 - (((first & 0x60) << 3) + second);
                run = ((first & 0x1C) >> 2) + 3 - 1;

                do
                {
                    destination[destinationIndex++] = destination[referenceIndex++];
                } while (run-- > 0);

                continue;
            }

            // int form
            if ((first & 0x40) == 0)
            {
                var second = compressedData[compressedDataIndex++];
                var third = compressedData[compressedDataIndex++];
                run = second >> 6;
                while (run-- > 0)
                {
                    destination[destinationIndex++] = compressedData[compressedDataIndex++];
                }

                var referenceIndex = destinationIndex - 1 - (((first & 0x3F) << 8) + third);
                run = (first & 0x3F) + 4 - 1;

                do
                {
                    destination[destinationIndex++] = destination[referenceIndex++];
                } while (run-- > 0);

                continue;
            }

            // very int form
            if ((first & 0x20) == 0)
            {
                var second = compressedData[compressedDataIndex++];
                var third = compressedData[compressedDataIndex++];
                var fourth = compressedData[compressedDataIndex++];
                run = first & 3;
                while (run-- > 0)
                {
                    destination[destinationIndex++] = compressedData[compressedDataIndex++];
                }

                var referenceIndex = destinationIndex - 1 - (((first & 0x10) >> 4 << 16) + (second << 8) + third);
                run = ((first & 0x0C) >> 2 << 8) + fourth + 5 - 1;

                do
                {
                    destination[destinationIndex++] = destination[referenceIndex++];
                } while (run-- > 0);

                continue;
            }

            run = ((first & 0x1F) << 2) + 4; // literal
            if (run <= 112)
            {
                while (run-- > 0)
                {
                    destination[destinationIndex++] = compressedData[compressedDataIndex++];
                }

                continue;
            }

            run = first & 3; // EOF (+0..3 literal)
            while (run-- > 0)
            {
                destination[destinationIndex++] = compressedData[compressedDataIndex++];
            }

            break;
        }

        return unpackedLength;
    }

    private static bool IsValid(ReadOnlySpan<byte> data)
    {
        var packType = data.ReadUInt16BigEndian();
        return packType is 0x10_FB or 0x11_FB or 0x90_FB or 0x91_FB;
    }

    private static int DecompressedSize(ReadOnlySpan<byte> data)
    {
        if (!IsValid(data))
        {
            throw new ArgumentException("Data is not a valid Refpack compressed stream.", nameof(data));
        }

        var packType = data.ReadUInt16BigEndian();
        var bytesToRead = (packType & 0x80_00) != 0 ? 4 : 3;
        var shouldSkipExtra = (packType & 0x01_00) != 0;
        var offset = shouldSkipExtra ? 2 + bytesToRead : 2;
        ReadOnlySpan<byte> array = data[offset..];
        return shouldSkipExtra ? (int)array.ReadUInt32BigEndian() : (int)array.ReadUInt24BigEndian();
    }
}
