// -----------------------------------------------------------------------
// <copyright file="SageCompressionStream.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.IO.Compression;
using SageCore.Extensions;

namespace SageCore.Io.Compression;

public sealed class SageCompressionStream : Stream
{
    private readonly Stream _baseStream;
    private readonly bool _leaveOpen;

    private bool _disposed;

    public SageCompressionStream(
        Stream stream,
        CompressionMode compressionMode,
        CompressionType compressionType = CompressionType.RefPack,
        bool leaveOpen = false
    )
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek)
        {
            throw new ArgumentException("The base stream must support seeking.", nameof(stream));
        }

        if (compressionMode is CompressionMode.Decompress && !stream.CanRead)
        {
            throw new ArgumentException("The base stream must be readable for decompression.", nameof(stream));
        }

        if (compressionMode is CompressionMode.Compress && !stream.CanWrite)
        {
            throw new ArgumentException("The base stream must be writable for compression.", nameof(stream));
        }

        _baseStream = stream;
        _leaveOpen = leaveOpen;

        CompressionMode = compressionMode;
        CompressionType = compressionType;

        if (CompressionMode is CompressionMode.Decompress)
        {
            CompressionType = DetectCompressionType(stream);
            if (CompressionType is CompressionType.None)
            {
                throw new InvalidDataException("The provided stream is not in a recognized compressed format.");
            }
        }
    }

    public static bool IsDataCompressed(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanRead)
        {
            throw new ArgumentException("The stream must be readable to check for compression.", nameof(stream));
        }

        if (stream.Length < 4)
        {
            return false;
        }

        var originalPosition = stream.Position;

        try
        {
            return DetectCompressionType(stream) is not CompressionType.None;
        }
        finally
        {
            stream.Position = originalPosition;
        }
    }

    public static CompressionType DetectCompressionType(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek)
        {
            throw new ArgumentException("The stream must support seeking to detect compression type.", nameof(stream));
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("The stream must be readable to detect compression type.", nameof(stream));
        }

        if (stream.Length < 4)
        {
            return CompressionType.None;
        }

        var originalPosition = stream.Position;
        stream.Position = 0;

        try
        {
            using BinaryReader reader = new(stream, LegacyEncodings.Ansi, leaveOpen: true);
            var magic = LegacyEncodings.Ansi.GetString(reader.ReadBytes(4));
            return magic switch
            {
                "EAR\0" => CompressionType.RefPack,
                "NOX\0" => CompressionType.NoxLzh,
                "ZL1\0" => CompressionType.ZLib1,
                "ZL2\0" => CompressionType.ZLib2,
                "ZL3\0" => CompressionType.ZLib3,
                "ZL4\0" => CompressionType.ZLib4,
                "ZL5\0" => CompressionType.ZLib5,
                "ZL6\0" => CompressionType.ZLib6,
                "ZL7\0" => CompressionType.ZLib7,
                "ZL8\0" => CompressionType.ZLib8,
                "ZL9\0" => CompressionType.ZLib9,
                "EAB\0" => CompressionType.BinaryTree,
                "EAH\0" => CompressionType.HuffmanWithRunlength,
                _ => CompressionType.None,
            };
        }
        finally
        {
            stream.Position = originalPosition;
        }
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

    public CompressionType CompressionType { get; }

    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting the length of a compression stream is not supported.");

    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException("Seeking in a compression stream is not supported.");

    public override int Read(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SageCompressionStream));
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (!CanRead)
        {
            throw new NotSupportedException("Reading is only supported in decompression mode.");
        }

        using Stream decompressionStream = CompressionType switch
        {
            CompressionType.None => throw new InvalidOperationException("The stream is not compressed."),
            CompressionType.RefPack => throw new NotImplementedException(),
            CompressionType.NoxLzh => throw new NotImplementedException(),
            CompressionType.ZLib1
            or CompressionType.ZLib2
            or CompressionType.ZLib3
            or CompressionType.ZLib4
            or CompressionType.ZLib5
            or CompressionType.ZLib6
            or CompressionType.ZLib7
            or CompressionType.ZLib8
            or CompressionType.ZLib9 => new ZLibStream(_baseStream, CompressionMode.Decompress, leaveOpen: true),
            CompressionType.BinaryTree => throw new NotImplementedException(),
            CompressionType.HuffmanWithRunlength => throw new NotImplementedException(),
            _ => throw new InvalidOperationException("Unsupported compression type for decompression."),
        };

        return decompressionStream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SageCompressionStream));
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (!CanWrite)
        {
            throw new NotSupportedException("Writing is only supported in compression mode.");
        }

        if (CompressionType is CompressionType.None)
        {
            _baseStream.Write(buffer, offset, count);
            return;
        }

        using Stream compressionStream = CompressionType switch
        {
            CompressionType.RefPack => throw new NotImplementedException(),
            CompressionType.NoxLzh => throw new NotImplementedException(),
            CompressionType.ZLib1
            or CompressionType.ZLib2
            or CompressionType.ZLib3
            or CompressionType.ZLib4
            or CompressionType.ZLib5
            or CompressionType.ZLib6
            or CompressionType.ZLib7
            or CompressionType.ZLib8
            or CompressionType.ZLib9 => new ZLibStream(
                _baseStream,
                (CompressionLevel)(CompressionType - CompressionType.ZLib1 + 1),
                leaveOpen: true
            ),
            CompressionType.BinaryTree => throw new NotImplementedException(),
            CompressionType.HuffmanWithRunlength => throw new NotImplementedException(),
            CompressionType.None => throw new InvalidOperationException(
                "The stream is not being compressed and somehow didn't get written directly before reaching the compression stage."
            ),
            _ => throw new InvalidOperationException("Unsupported compression type for compression."),
        };

        compressionStream.Write(buffer, offset, count);
    }

    public override void Flush()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SageCompressionStream));
        if (!CanWrite)
        {
            throw new NotSupportedException("Flushing is only supported in compression mode.");
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
