// -----------------------------------------------------------------------
// <copyright file="LzhLightStream.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.IO.Compression;
using SageCore.Extensions;
using SageCore.Io.Compression.LzhLight.Internals;

namespace SageCore.Io.Compression.LzhLight;

internal sealed class LzhLightStream : Stream
{
    private readonly Stream _baseStream;
    private readonly bool _leaveOpen;

    private bool _disposed;

    public LzhLightStream(Stream stream, CompressionMode compressionMode, bool leaveOpen = false)
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

    public override bool CanRead => CompressionMode is CompressionMode.Decompress && !_disposed && _baseStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => CompressionMode is CompressionMode.Compress && !_disposed && _baseStream.CanWrite;

    public override long Length => _baseStream.Length;

    public override long Position
    {
        get => _baseStream.Position;
        set => throw new NotSupportedException("Setting the position of a compression stream is not supported.");
    }

    public CompressionMode CompressionMode { get; }

    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting the length of a compression stream is not supported.");

    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException("Seeking in a compression stream is not supported.");

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

        if (count == 0)
        {
            return 0;
        }

        var destinationBytesWritten = 0;
        var sourceBytesRead = 0;
        Decompressor decompressor = new();

        using BinaryReader reader = new(_baseStream, LegacyEncodings.Ansi, leaveOpen: true);
        var source = reader.ReadBytes(count);

        return !decompressor.Decompress(
            buffer.AsSpan(offset, count),
            ref destinationBytesWritten,
            source,
            ref sourceBytesRead
        )
            ? throw new InvalidDataException("The data is not in a valid LZH-Lite format or is corrupted.")
            : destinationBytesWritten;
    }

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

        Compressor compressor = new();

        using BinaryReader reader = new(_baseStream, LegacyEncodings.Ansi, leaveOpen: true);
        var source = reader.ReadBytes(count);

        _ = compressor.Compress(buffer.AsSpan(offset, count), source);
    }

    public override void Flush()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!CanWrite)
        {
            throw new NotSupportedException("Flushing is only supported when the stream is writable.");
        }

        _baseStream.Flush();
    }

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
}
