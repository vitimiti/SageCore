// -----------------------------------------------------------------------
// <copyright file="SageCompressionStream.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using SageCore.Compression.Lzhl;

namespace SageCore.Compression;

/// <summary>
/// A stream that compresses or decompresses data using a specified compression algorithm for the SageCore engine.
/// </summary>
public sealed class SageCompressionStream : Stream
{
    private readonly Stream _baseStream;
    private readonly CompressionType _compressionType;
    private readonly CompressionMode _compressionMode;
    private readonly bool _leaveOpen;

    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SageCompressionStream"/> class.
    /// </summary>
    /// <param name="baseStream">The base stream to compress or decompress.</param>
    /// <param name="compressionType">The compression type to use.</param>
    /// <param name="leaveOpen">A value indicating whether to leave the base stream open when this stream is disposed.</param>
    /// <exception cref="ArgumentException">The base stream must be seekable.</exception>
    /// <exception cref="ArgumentException">The base stream must be writable when in compression mode.</exception>
    /// <exception cref="ArgumentException">The base stream must be readable when in decompression mode.</exception>
    /// <remarks>If the stream is opened in compression mode, the compression type must be specified.</remarks>
    public SageCompressionStream([NotNull] Stream baseStream, CompressionType compressionType, bool leaveOpen = false)
    {
        if (!baseStream.CanSeek)
        {
            throw new ArgumentException("The base stream must be seekable.", nameof(baseStream));
        }

        _baseStream = baseStream;
        _compressionType = compressionType;
        _compressionMode = CompressionMode.Compress;
        _leaveOpen = leaveOpen;

        if (!baseStream.CanWrite)
        {
            throw new ArgumentException("The base stream must be writable.", nameof(baseStream));
        }

        if (!baseStream.CanSeek)
        {
            throw new ArgumentException("The base stream must be seekable.", nameof(baseStream));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageCompressionStream"/> class.
    /// </summary>
    /// <param name="baseStream">The base stream to compress or decompress.</param>
    /// <param name="compressionMode">The compression mode.</param>
    /// <param name="leaveOpen">A value indicating whether to leave the base stream open when this stream is disposed.</param>
    /// <exception cref="ArgumentException">The base stream must be seekable.</exception>
    /// <exception cref="ArgumentException">The base stream must be readable when in decompression mode.</exception>
    /// <exception cref="ArgumentException">The base stream must be writable when in compression mode.</exception>
    /// <remarks>If the stream is opened in decompression mode, the compression type is auto-detected from the stream header.</remarks>
    public SageCompressionStream([NotNull] Stream baseStream, CompressionMode compressionMode, bool leaveOpen = false)
    {
        if (!baseStream.CanSeek)
        {
            throw new ArgumentException("The base stream must be seekable.", nameof(baseStream));
        }

        _baseStream = baseStream;
        MemoryStream mem = new();
        _baseStream.CopyTo(mem);

        // If we are decompressing, autodetect the compression type from the stream header.
        // If we are compressing, default to None (no compression).
        _compressionType =
            compressionMode is CompressionMode.Compress ? CompressionType.None : DetectCompressionType(mem.ToArray());

        _baseStream.Position = 0;
        _compressionMode = compressionMode;
        _leaveOpen = leaveOpen;

        if (!baseStream.CanRead && compressionMode == CompressionMode.Decompress)
        {
            throw new ArgumentException("The base stream must be readable.", nameof(baseStream));
        }

        if (!baseStream.CanWrite && compressionMode == CompressionMode.Compress)
        {
            throw new ArgumentException("The base stream must be writable.", nameof(baseStream));
        }

        if (!baseStream.CanSeek)
        {
            throw new ArgumentException("The base stream must be seekable.", nameof(baseStream));
        }
    }

    /// <summary>
    /// Gets a value indicating whether the current stream supports reading.
    /// </summary>
    /// <remarks>The stream supports reading if it was opened in decompression mode and the underlying stream is readable.</remarks>
    /// <inheritdoc/>
    public override bool CanRead => _compressionMode == CompressionMode.Decompress;

    /// <summary>
    /// Gets a value indicating whether the current stream supports seeking.
    /// </summary>
    /// <remarks>The stream does not support seeking.</remarks>
    /// <inheritdoc/>
    public override bool CanSeek => false;

    /// <summary>
    /// Gets a value indicating whether the current stream supports writing.
    /// </summary>
    /// <remarks>The stream supports writing if it was opened in compression mode and the underlying stream is writable.</remarks>
    /// <inheritdoc/>
    public override bool CanWrite => _compressionMode == CompressionMode.Compress;

    /// <inheritdoc/>
    public override long Length => _baseStream.Length;

    /// <summary>
    /// Gets or sets the position within the current stream.
    /// </summary>
    /// <exception cref="NotSupportedException">Setting the position is not supported.</exception>
    /// <inheritdoc/>
    public override long Position
    {
        get => _baseStream.Position;
        set => throw new NotSupportedException("Setting the position is not supported.");
    }

    /// <summary>
    /// Flushes the underlying stream.
    /// </summary>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="NotSupportedException">The stream is not writable.</exception>
    public override void Flush()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!CanWrite)
        {
            throw new NotSupportedException("The stream is not writable.");
        }

        _baseStream.Flush();
    }

    public override int Read([NotNull] byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (!CanRead)
        {
            throw new NotSupportedException("The stream is not readable.");
        }

        if (_compressionType is CompressionType.None)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        if (_compressionType is CompressionType.NoxLzh)
        {
            (var expectedDecompressedInt, var compressed) = ReadHeaderAndPayload(t => t == CompressionType.NoxLzh);

            Decompressor decompressor = new();
            Span<byte> callerSpan = buffer.AsSpan(offset, count);

            // Try decompress directly into caller span if it fits
            if (count >= expectedDecompressedInt)
            {
                return !decompressor.TryDecompress(
                    callerSpan[..expectedDecompressedInt],
                    compressed,
                    out var bytesWritten
                )
                    ? throw new InvalidDataException("NOXLZH Decompression failed.")
                    : bytesWritten;
            }

            // Otherwise decompress into a temporary buffer then copy
            var decompressed = new byte[expectedDecompressedInt];
            if (!decompressor.TryDecompress(decompressed, compressed, out var bytesWritten2))
            {
                throw new InvalidDataException("NOXLZH Decompression failed.");
            }

            var toCopy = Math.Min(bytesWritten2, count);
            if (toCopy > 0)
            {
                Array.Copy(decompressed, 0, buffer, offset, toCopy);
            }

            return toCopy;
        }

        if (
            _compressionType
            is CompressionType.ZLib1
                or CompressionType.ZLib2
                or CompressionType.ZLib3
                or CompressionType.ZLib4
                or CompressionType.ZLib5
                or CompressionType.ZLib6
                or CompressionType.ZLib7
                or CompressionType.ZLib8
                or CompressionType.ZLib9
        )
        {
            (var expectedDecompressedInt, var compressed) = ReadHeaderAndPayload(t =>
                t
                    is CompressionType.ZLib1
                        or CompressionType.ZLib2
                        or CompressionType.ZLib3
                        or CompressionType.ZLib4
                        or CompressionType.ZLib5
                        or CompressionType.ZLib6
                        or CompressionType.ZLib7
                        or CompressionType.ZLib8
                        or CompressionType.ZLib9
            );

            Span<byte> callerSpan = buffer.AsSpan(offset, count);
            using ZLibStream zlib = new(new MemoryStream(compressed), CompressionMode.Decompress, leaveOpen: true);

            if (count >= expectedDecompressedInt)
            {
                return zlib.Read(callerSpan[..expectedDecompressedInt]);
            }

            var decompressed = new byte[expectedDecompressedInt];
            var bytesRead = zlib.Read(decompressed);
            var toCopy2 = Math.Min(bytesRead, count);
            if (toCopy2 > 0)
            {
                Array.Copy(decompressed, 0, buffer, offset, toCopy2);
            }

            return toCopy2;
        }

        throw new NotSupportedException($"The compression type '{_compressionType}' is not supported.");
    }

    /// <summary>
    /// Seeking is not supported.
    /// </summary>
    /// <inheritdoc/>
    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException("Seeking is not supported.");

    /// <summary>
    /// Setting the length is not supported.
    /// </summary>
    /// <inheritdoc/>
    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting the length is not supported.");

    public override void Write([NotNull] byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (!CanWrite)
        {
            throw new NotSupportedException("The stream is not writable.");
        }

        if (_compressionType is CompressionType.None)
        {
            _baseStream.Write(buffer, offset, count);
            return;
        }

        if (_compressionType is CompressionType.NoxLzh)
        {
            ReadOnlySpan<byte> source = buffer.AsSpan(offset, count);

            Compressor compressor = new();

            var maxDestination = EstimateMaxCompressedSize(count, _compressionType);
            if (maxDestination < 0)
            {
                throw new InvalidOperationException("Invalid estimated compressed size.");
            }

            var destination = new byte[maxDestination];

            var compressedSize = (int)compressor.Compress(destination, source);
            if (compressedSize <= 0 || compressedSize > maxDestination)
            {
                throw new InvalidDataException("NOXLZH Compression failed.");
            }

            WriteHeaderAndPayload("NOX\0"u8, count, destination, compressedSize);
            return;
        }

        if (
            _compressionType
            is CompressionType.ZLib1
                or CompressionType.ZLib2
                or CompressionType.ZLib3
                or CompressionType.ZLib4
                or CompressionType.ZLib5
                or CompressionType.ZLib6
                or CompressionType.ZLib7
                or CompressionType.ZLib8
                or CompressionType.ZLib9
        )
        {
            ReadOnlySpan<byte> source = buffer.AsSpan(offset, count);

            // Use helper to estimate destination buffer size to avoid overruns
            var maxDestination = EstimateMaxCompressedSize(count, _compressionType);
            if (maxDestination < 0)
            {
                throw new InvalidOperationException("Invalid estimated compressed size.");
            }

            var destination = new byte[maxDestination];
            int compressedSize;
            using (MemoryStream ms = new(destination))
            {
                using (
                    ZLibStream zlib = new(
                        ms,
                        (CompressionLevel)((int)_compressionType - (int)CompressionType.ZLib1 + 1),
                        leaveOpen: true
                    )
                )
                {
                    zlib.Write(source);
                }

                compressedSize = (int)ms.Position;
            }

            if (compressedSize <= 0 || compressedSize > maxDestination)
            {
                throw new InvalidDataException("ZLib Compression failed.");
            }

            ReadOnlySpan<byte> signature = _compressionType switch
            {
                CompressionType.ZLib1 => "ZL1\0"u8,
                CompressionType.ZLib2 => "ZL2\0"u8,
                CompressionType.ZLib3 => "ZL3\0"u8,
                CompressionType.ZLib4 => "ZL4\0"u8,
                CompressionType.ZLib5 => "ZL5\0"u8,
                CompressionType.ZLib6 => "ZL6\0"u8,
                CompressionType.ZLib7 => "ZL7\0"u8,
                CompressionType.ZLib8 => "ZL8\0"u8,
                CompressionType.ZLib9 => "ZL9\0"u8,
                CompressionType.None => throw new InvalidOperationException("Invalid compression type."),
                CompressionType.RefPack => throw new InvalidOperationException("Invalid compression type."),
                CompressionType.NoxLzh => throw new InvalidOperationException("Invalid compression type."),
                CompressionType.BinaryTree => throw new InvalidOperationException("Invalid compression type."),
                CompressionType.HuffmanWithRunLength => throw new InvalidOperationException(
                    "Invalid compression type."
                ),
                _ => throw new InvalidOperationException("Invalid compression type."),
            };

            WriteHeaderAndPayload(signature, count, destination, compressedSize);
            return;
        }

        throw new NotSupportedException($"The compression type '{_compressionType}' is not supported.");
    }

    /// <inheritdoc/>
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

    private static CompressionType DetectCompressionType(ReadOnlySpan<byte> buffer) =>
        buffer.Length < 8 ? CompressionType.None
        : buffer.StartsWith("NOX\0"u8) ? CompressionType.NoxLzh
        : buffer.StartsWith("ZL1\0"u8) ? CompressionType.ZLib1
        : buffer.StartsWith("ZL2\0"u8) ? CompressionType.ZLib2
        : buffer.StartsWith("ZL3\0"u8) ? CompressionType.ZLib3
        : buffer.StartsWith("ZL4\0"u8) ? CompressionType.ZLib4
        : buffer.StartsWith("ZL5\0"u8) ? CompressionType.ZLib5
        : buffer.StartsWith("ZL6\0"u8) ? CompressionType.ZLib6
        : buffer.StartsWith("ZL7\0"u8) ? CompressionType.ZLib7
        : buffer.StartsWith("ZL8\0"u8) ? CompressionType.ZLib8
        : buffer.StartsWith("ZL9\0"u8) ? CompressionType.ZLib9
        : buffer.StartsWith("EAB\0"u8) ? CompressionType.BinaryTree
        : buffer.StartsWith("EAH\0"u8) ? CompressionType.HuffmanWithRunLength
        : buffer.StartsWith("EAR\0"u8) ? CompressionType.RefPack
        : CompressionType.None;

    private static int EstimateMaxCompressedSize(int uncompressedLength, CompressionType compressionType) =>
        compressionType switch
        {
            CompressionType.NoxLzh => Compressor.CalculateMaximumBufferSize(uncompressedLength),
            CompressionType.BinaryTree or CompressionType.HuffmanWithRunLength or CompressionType.RefPack =>
                uncompressedLength + 8, // Guessing here
            CompressionType.ZLib1
            or CompressionType.ZLib2
            or CompressionType.ZLib3
            or CompressionType.ZLib4
            or CompressionType.ZLib5
            or CompressionType.ZLib6
            or CompressionType.ZLib7
            or CompressionType.ZLib8
            or CompressionType.ZLib9 => (int)float.Ceiling((float)((uncompressedLength * 1.1) + 12 + 8)), // zlib's compressBound
            CompressionType.None => uncompressedLength,
            _ => uncompressedLength,
        };

    private static int CalculateUncompressedSize(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 8)
        {
            return buffer.Length;
        }

        CompressionType compressionType = DetectCompressionType(buffer);
        return compressionType switch
        {
            CompressionType.NoxLzh
            or CompressionType.ZLib1
            or CompressionType.ZLib2
            or CompressionType.ZLib3
            or CompressionType.ZLib4
            or CompressionType.ZLib5
            or CompressionType.ZLib6
            or CompressionType.ZLib7
            or CompressionType.ZLib8
            or CompressionType.ZLib9
            or CompressionType.BinaryTree
            or CompressionType.HuffmanWithRunLength
            or CompressionType.RefPack => BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(4, 4)),
            CompressionType.None => buffer.Length,
            _ => buffer.Length,
        };
    }

    private (int ExpectedUncompressedSize, byte[] CompressedPayload) ReadHeaderAndPayload(
        Func<CompressionType, bool> typePredicate
    )
    {
        Span<byte> header = stackalloc byte[8];
        _baseStream.ReadExactly(header);

        CompressionType detected = DetectCompressionType(header);
        if (!typePredicate(detected))
        {
            throw new InvalidDataException("Unexpected compression type in header.");
        }

        var expected = CalculateUncompressedSize(header);

        var remaining = _baseStream.Length - _baseStream.Position;
        if (remaining is < 0 or > int.MaxValue)
        {
            throw new InvalidDataException($"Invalid compressed payload length of {remaining}.");
        }

        var compressedLength = (int)remaining;
        var compressed = new byte[compressedLength];
        var totalRead = 0;
        while (totalRead < compressedLength)
        {
            var n = _baseStream.Read(compressed, totalRead, compressedLength - totalRead);
            if (n == 0)
            {
                throw new EndOfStreamException("Unexpected end of stream while reading compressed payload.");
            }

            totalRead += n;
        }

        return (expected, compressed);
    }

    private void WriteHeaderAndPayload(
        ReadOnlySpan<byte> signature,
        int uncompressedSize,
        byte[] payload,
        int payloadLength
    )
    {
        if (signature.Length != 4)
        {
            throw new ArgumentException("Signature must be 4 bytes.", nameof(signature));
        }

        _baseStream.Write(signature);
        Span<byte> sizeBytes = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32LittleEndian(sizeBytes, (uint)uncompressedSize);
        _baseStream.Write(sizeBytes);
        _baseStream.Write(payload, 0, payloadLength);
    }
}
