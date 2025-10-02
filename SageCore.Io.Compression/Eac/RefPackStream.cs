// -----------------------------------------------------------------------
// <copyright file="RefPackStream.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.IO.Compression;
using SageCore.Extensions;

namespace SageCore.Io.Compression.Eac;

/// <summary>
/// Represents a stream that can be used for reading and writing compressed data using the RefPack algorithm.
/// </summary>
public sealed class RefPackStream : Stream
{
    private readonly Stream _baseStream;
    private readonly bool _leaveOpen;

    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefPackStream"/> class with the specified stream and compression mode.
    /// </summary>
    /// <param name="stream">The stream to read from or write to.</param>
    /// <param name="compressionMode">The compression mode to use (compress or decompress).</param>
    /// <param name="leaveOpen">If set to <see langword="true"/>, the underlying stream is left open after the <see cref="RefPackStream"/> is disposed; otherwise, it is closed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="stream"/> does not support seeking.-or-<paramref name="compressionMode"/> is <see cref="CompressionMode.Compress"/> and <paramref name="stream"/> is not writable.-or-<paramref name="compressionMode"/> is <see cref="CompressionMode.Decompress"/> and <paramref name="stream"/> is not readable.</exception>
    public RefPackStream(Stream stream, CompressionMode compressionMode, bool leaveOpen = false)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek)
        {
            throw new ArgumentException("The stream must support seeking.", nameof(stream));
        }

        if (compressionMode is CompressionMode.Compress && !stream.CanWrite)
        {
            throw new ArgumentException("The stream must be writable when compressing.", nameof(stream));
        }

        if (compressionMode is CompressionMode.Decompress && !stream.CanRead)
        {
            throw new ArgumentException("The stream must be readable when decompressing.", nameof(stream));
        }

        _baseStream = stream;
        _leaveOpen = leaveOpen;

        CompressionMode = compressionMode;
    }

    /// <summary>
    /// Gets a value indicating whether the current stream supports reading.
    /// </summary>
    public override bool CanRead => CompressionMode is CompressionMode.Decompress && !_disposed && _baseStream.CanRead;

    /// <summary>
    /// Gets a value indicating whether the current stream supports seeking. Always returns <see langword="false"/>.
    /// </summary>
    public override bool CanSeek => false;

    /// <summary>
    /// Gets a value indicating whether the current stream supports writing.
    /// </summary>
    public override bool CanWrite => CompressionMode is CompressionMode.Compress && !_disposed && _baseStream.CanWrite;

    /// <summary>
    /// Gets the length in bytes of the stream.
    /// </summary>
    public override long Length => _baseStream.Length;

    /// <summary>
    /// Gets or sets the position within the current stream.
    /// </summary>
    /// <exception cref="NotSupportedException">Setting the position is not supported.</exception>
    public override long Position
    {
        get => _baseStream.Position;
        set => throw new NotSupportedException("Setting the position of a compression stream is not supported.");
    }

    /// <summary>
    /// Gets the compression mode of the stream.
    /// </summary>
    public CompressionMode CompressionMode { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to use quick compression.
    /// </summary>
    public bool QuickCompression { get; set; }

    /// <summary>
    /// Determines if the provided stream contains valid RefPack compressed data by checking its header.
    /// </summary>
    /// <param name="stream">The stream to check.</param>
    /// <returns><see langword="true"/> if the stream contains valid RefPack compressed data; otherwise, <see langword="false"/>.</returns>
    public static bool IsValidRefPackStream(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek)
        {
            throw new ArgumentException("The stream must support seeking.", nameof(stream));
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("The stream must be readable.", nameof(stream));
        }

        var originalPosition = stream.Position;

        try
        {
            using BinaryReader reader = new(stream, LegacyEncodings.Ansi, leaveOpen: true);
            var header = reader.ReadUInt16BigEndian();
            return header is 0x10FB or 0x11FB or 0x90FB or 0x91FB;
        }
        finally
        {
            stream.Position = originalPosition;
        }
    }

    /// <summary>
    /// Calculates the uncompressed size of the data in a RefPack compressed stream without fully decompressing it.
    /// </summary>
    /// <param name="stream">The RefPack compressed stream.</param>
    /// <returns>The uncompressed size of the data.</returns>
    public static int CalculateUncompressedSize(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek)
        {
            throw new ArgumentException("The stream must support seeking.", nameof(stream));
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("The stream must be readable.", nameof(stream));
        }

        if (!IsValidRefPackStream(stream))
        {
            throw new InvalidDataException("The stream does not contain valid RefPack compressed data.");
        }

        var originalPosition = stream.Position;

        try
        {
            using BinaryReader reader = new(stream, LegacyEncodings.Ansi, leaveOpen: true);
            var header = reader.ReadUInt16BigEndian();
            var bytesToRead = (header & 0x80_00) != 0 ? 4 : 3;
            var offset = (header & 0x01_00) != 0 ? 2 + bytesToRead : 2;

            // Create the offset
            _ = reader.ReadBytes(offset);

            // We know it HAS to be 4 or 3 here
            return bytesToRead switch
            {
                4 => (int)reader.ReadUInt32BigEndian(),
                _ => (int)reader.ReadUInt24BigEndian(),
            };
        }
        finally
        {
            stream.Position = originalPosition;
        }
    }

    /// <summary>
    /// Not supported.
    /// </summary>
    /// <param name="value">The desired length of the current stream in bytes.</param>
    /// <exception cref="NotSupportedException">Always thrown since setting the length is not supported.</exception>
    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting the length of a compression stream is not supported.");

    /// <summary>
    /// Not supported.
    /// </summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
    /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
    /// <returns>The new position within the current stream.</returns>
    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException("Seeking in a compression stream is not supported.");

    /// <summary>
    /// Reads a sequence of bytes from the current RefPack compressed stream and advances the position within the stream by the number of bytes read.
    /// </summary>
    /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.-or-<paramref name="offset"/> + <paramref name="count"/> is greater than the length of <paramref name="buffer"/>.</exception>
    /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="InvalidDataException">The stream does not contain valid RefPack compressed data.</exception>
    /// <remarks>
    /// This method fully decompresses the RefPack data into memory before copying the requested number of bytes into the provided buffer.
    /// </remarks>
    public override int Read(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (!CanRead)
        {
            throw new NotSupportedException("Decompressing is only supported when the stream is readable.");
        }

        if (!IsValidRefPackStream(_baseStream))
        {
            throw new InvalidDataException("The stream does not contain valid RefPack compressed data.");
        }

        if (count == 0)
        {
            return 0;
        }

        List<byte> output = [];
        using BinaryReader reader = new(_baseStream, LegacyEncodings.Ansi, leaveOpen: true);

        var type = reader.ReadByte();
        type = unchecked((byte)((type << 8) + reader.ReadByte()));

        var uncompressedLength = 0;

        // 4-byte size field
        if ((type & 0x80_00) != 0)
        {
            // Skip ulen
            if ((type & 0x01_00) != 0)
            {
                _ = reader.ReadBytes(4);
            }

            uncompressedLength = reader.ReadByte();
            uncompressedLength = (uncompressedLength << 8) + reader.ReadByte();
        }
        else
        {
            // Skip ulen
            if ((type & 0x01_00) != 0)
            {
                _ = reader.ReadBytes(3);
            }

            uncompressedLength = reader.ReadByte();
        }

        uncompressedLength = (uncompressedLength << 8) + reader.ReadByte();
        uncompressedLength = (uncompressedLength << 8) + reader.ReadByte();
        output.Capacity = uncompressedLength;

        while (true)
        {
            var first = reader.ReadByte();
            var runlength = 0;

            // Short form
            if ((first & 0x80) == 0)
            {
                var second = reader.ReadByte();
                runlength = first & 3;
                while (runlength-- > 0)
                {
                    output.Add(reader.ReadByte());
                }

                var referenceIndex = output.Count - 1 - (((first & 0x60) << 3) + second);
                runlength = ((first & 0x1C) >> 2) + 3 - 1;

                do
                {
                    output.Add(output[referenceIndex++]);
                } while (runlength-- > 0);
                continue;
            }

            // Int form
            if ((first & 0x40) == 0)
            {
                var second = reader.ReadByte();
                var third = reader.ReadByte();
                runlength = second >> 6;
                while (runlength-- >= 0)
                {
                    output.Add(reader.ReadByte());
                }

                var referenceIndex = output.Count - 1 - (((second & 0x3F) << 8) + third);
                runlength = (first & 0x3F) + 4 - 1;

                do
                {
                    output.Add(output[referenceIndex++]);
                } while (runlength-- > 0);
                continue;
            }

            // Very int form
            if ((first & 0x20) == 0)
            {
                var second = reader.ReadByte();
                var third = reader.ReadByte();
                var fourth = reader.ReadByte();
                runlength = first & 3;
                while (runlength-- > 0)
                {
                    output.Add(reader.ReadByte());
                }

                var referenceIndex = output.Count - 1 - (((first & 0x10) >> 4 << 16) + (second << 8) + third);
                runlength = ((first & 0x0C) >> 2 << 8) + fourth + 5 - 1;

                do
                {
                    output.Add(output[referenceIndex++]);
                } while (runlength-- > 0);
                continue;
            }

            // Literal
            runlength = ((first & 0x1F) << 2) + 4;
            if (runlength <= 112)
            {
                while (runlength-- > 0)
                {
                    output.Add(reader.ReadByte());
                }

                continue;
            }

            // EOF (+0..3 literal)
            runlength = first & 3;
            while (runlength-- > 0)
            {
                output.Add(reader.ReadByte());
            }

            break;
        }

        var bytesToCopy = int.Min(count, output.Count);
        Array.Copy(output.ToArray(), 0, buffer, offset, bytesToCopy);
        return bytesToCopy;
    }

    /// <summary>
    /// Writes the specified byte array to the current stream as compressed data.
    /// </summary>
    /// <param name="buffer">The byte array to write.</param>
    /// <param name="offset">The zero-based byte offset in the array at which to begin copying bytes.</param>
    /// <param name="count">The number of bytes to write.</param>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.-or-<paramref name="offset"/> + <paramref name="count"/> is greater than the length of <paramref name="buffer"/>.</exception>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (!CanWrite)
        {
            throw new NotSupportedException("Compressing is only supported when the stream is writable.");
        }

        if (count > 0xFF_FF_FF) // 32-bit header required
        {
            using BinaryWriter writer = new(_baseStream, LegacyEncodings.Ansi, leaveOpen: true);
            writer.WriteUInt16BigEndian(0x90FB);
            writer.WriteUInt32BigEndian((uint)count);
        }
        else
        {
            using BinaryWriter writer = new(_baseStream, LegacyEncodings.Ansi, leaveOpen: true);
            writer.WriteUInt16BigEndian(0x10FB);
            writer.WriteUInt24BigEndian((uint)count);
        }

        if (count == 0)
        {
            return; // Nothing to do
        }

        var total = offset + count;
        var payload = RefCompress(buffer[offset..total], count);

        List<byte> output = [];
        output.Capacity = payload.Length + 8;

        output.AddRange(payload);
        _baseStream.Write([.. output], 0, output.Count);
    }

    /// <summary>
    /// Flushes the underlying stream if the current stream is writable.
    /// </summary>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    public override void Flush()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!CanWrite)
        {
            throw new NotSupportedException("Flushing is only supported when the stream is writable.");
        }

        _baseStream.Flush();
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="RefPackStream"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && !_leaveOpen)
        {
            _baseStream.Dispose();
        }

        _disposed = true;
        base.Dispose(disposing);
    }

    private static int MatchLength(
        byte[] source,
        int sourceIndex,
        byte[] destination,
        int destinationIndex,
        int maxmatch
    )
    {
        var current = 0;
        var sLimit = source.Length;
        var dLimit = destination.Length;
        while (
            current < maxmatch
            && sourceIndex + current < sLimit
            && destinationIndex + current < dLimit
            && source[sourceIndex + current] == destination[destinationIndex + current]
        )
        {
            current++;
        }

        return current;
    }

    private static int HashAt(byte[] data, int index)
    {
        if (data.Length < 3)
        {
            throw new ArgumentException("Data must be at least 3 bytes long to compute hash.", nameof(data));
        }

        var first = data[index];
        var second = data[index + 1];
        var third = data[index + 2];
        return unchecked(((first << 8) | third) ^ (second << 4));
    }

    private byte[] RefCompress(byte[] buffer, int count)
    {
        const int maxBack = 0x01_FF_FF;
        List<byte> output = [];
        output.Capacity = count;

        var run = 0;
        var cIndex = 0;
        var rIndex = 0;
        var length = buffer.Length;

        var hashTable = new int[0x01_00_00];
        var link = new int[0x02_00_00];

        Array.Fill(hashTable, -1);
        var remaining = length - 4;
        while (remaining >= 0)
        {
            var bOffset = 0;
            var bLength = 2;
            var bCost = 2;
            var minLength = int.Min(remaining, 0x04_04);
            var hash = HashAt(buffer, cIndex);
            var hashOffset = hashTable[hash];
            var minHashOffset = int.Max(cIndex - maxBack, 0);
            if (hashOffset >= minHashOffset)
            {
                var iter = hashOffset;
                while (iter >= minHashOffset)
                {
                    var tIndex = iter;
                    if (
                        cIndex + bLength < buffer.Length
                        && tIndex + bLength < buffer.Length
                        && buffer[cIndex + bLength] == buffer[tIndex + bLength]
                    )
                    {
                        var tLength = MatchLength(buffer, cIndex, buffer, tIndex, minLength);
                        if (tLength > bLength)
                        {
                            var tOffset = cIndex - 1 - tIndex;
                            var tCost = tOffset switch
                            {
                                < 0x04_00 when tLength <= 0x0A => 2,
                                < 0x40_00 when tLength <= 0x43 => 3,
                                _ => 4,
                            };

                            if (tLength - tCost + 4 > bLength - bCost + 4)
                            {
                                bLength = tLength;
                                bCost = tCost;
                                bOffset = tOffset;
                                if (bLength >= 0x04_04)
                                {
                                    break;
                                }
                            }
                        }

                        iter = link[iter & maxBack];
                        if (iter < minHashOffset)
                        {
                            break;
                        }
                    }
                }

                if (bCost >= bLength || remaining < 4)
                {
                    var hoff = cIndex;
                    var slot = HashAt(buffer, cIndex);
                    link[hoff & maxBack] = hashTable[slot];
                    hashTable[slot] = hoff;

                    run++;
                    cIndex++;
                    remaining--;
                }
                else
                {
                    // Literal block of data
                    while (run > 3)
                    {
                        var tLength = int.Min(112, run & (~3));
                        run -= tLength;
                        output.Add(unchecked((byte)(0xE0 + (tLength >> 2) - 1)));
                        for (var i = 0; i < tLength; i++)
                        {
                            output.Add(buffer[rIndex + i]);
                        }

                        rIndex += tLength;
                    }

                    if (bCost == 2) // 2-byte int form
                    {
                        output.Add(unchecked((byte)(((bOffset >> 8) << 5) + ((bLength - 3) << 2) + run)));
                        output.Add(unchecked((byte)bOffset));
                    }
                    else if (bCost == 3) // 3-byte int form
                    {
                        output.Add(unchecked((byte)(0x80 + (bLength - 4))));
                        output.Add(unchecked((byte)((run << 6) + (bOffset >> 8))));
                        output.Add(unchecked((byte)bOffset));
                    }
                    else // 4-byte very int form
                    {
                        output.Add(
                            unchecked((byte)(0xC0 + ((bOffset >> 16) << 4) + (((bLength - 5) >> 8) << 2) + run))
                        );

                        output.Add(unchecked((byte)(bOffset >> 8)));
                        output.Add(unchecked((byte)bOffset));
                        output.Add(unchecked((byte)(bLength - 5)));
                    }

                    if (run > 0)
                    {
                        for (var i = 0; i < run; i++)
                        {
                            output.Add(buffer[rIndex + i]);
                            rIndex += run;
                            run = 0;
                        }
                    }

                    if (QuickCompression)
                    {
                        var hoff = cIndex;
                        var slot = HashAt(buffer, cIndex);
                        link[hoff & maxBack] = hashTable[slot];
                        hashTable[slot] = hoff;
                        cIndex += bLength;
                    }
                    else
                    {
                        for (var i = 0; i < bLength; i++)
                        {
                            var slot = HashAt(buffer, cIndex);
                            var hoff = cIndex;
                            link[hoff & maxBack] = hashTable[slot];
                            hashTable[slot] = hoff;
                            cIndex++;
                        }
                    }

                    rIndex = cIndex;
                    remaining -= bLength;
                }
            }
        }

        remaining += 4;
        run += remaining;

        // Not match at end, use literal
        while (run > 3)
        {
            var tLength = int.Min(112, run & (~3));
            run -= tLength;
            output.Add(unchecked((byte)(0xE0 + (tLength >> 2) - 1)));
            for (var i = 0; i < tLength; i++)
            {
                output.Add(buffer[rIndex + i]);
            }

            rIndex += tLength;
        }

        // EOF (+0..3 literal)
        output.Add(unchecked((byte)(0xFC + run)));
        if (run > 0)
        {
            for (var i = 0; i < run; i++)
            {
                output.Add(buffer[rIndex + i]);
            }
        }

        return [.. output];
    }
}
