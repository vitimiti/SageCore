// -----------------------------------------------------------------------
// <copyright file="SageCompressionStream.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.IO.Compression;
using SageCore.Extensions;
using SageCore.Io.Compression.Eac;

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

    public static int CalculateUncompressedSize(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanSeek)
        {
            throw new ArgumentException(
                "The stream must support seeking to calculate uncompressed size.",
                nameof(stream)
            );
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("The stream must be readable to calculate uncompressed size.", nameof(stream));
        }

        if (stream.Length < 8)
        {
            throw new InvalidDataException("The stream is too short to contain valid compressed data.");
        }

        var originalPosition = stream.Position;
        stream.Position = 0;

        try
        {
            using BinaryReader reader = new(stream, LegacyEncodings.Ansi, leaveOpen: true);
            _ = reader.ReadBytes(4); // Skip magic bytes
            return reader.ReadInt32();
        }
        finally
        {
            stream.Position = originalPosition;
        }
    }

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

        if (CompressionType is CompressionType.None)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        using Stream decompressionStream = CompressionType switch
        {
            CompressionType.None => throw new InvalidOperationException(
                "The stream is not compressed and somehow reached the decompression stage."
            ),
            CompressionType.RefPack => new RefPackStream(_baseStream, CompressionMode.Decompress, leaveOpen: true),
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

        // Read and discard the header (magic bytes + uncompressed size).
        using (BinaryReader reader = new(decompressionStream, LegacyEncodings.Ansi, leaveOpen: true))
        {
            _ = reader.ReadBytes(4); // Skip magic bytes
            _ = reader.ReadInt32(); // Skip uncompressed size
        }

        // The first 8 bytes have been read for the header, so we need to adjust the buffer accordingly.
        return decompressionStream.Read(buffer, offset + 8, count - 8);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SageCompressionStream));
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        if (CompressionType is CompressionType.None)
        {
            _baseStream.Write(buffer, offset, count);
            return;
        }

        if (count < 8)
        {
            throw new ArgumentException(
                "At least 8 bytes must be written to determine compression type and data length.",
                nameof(count)
            );
        }

        if (!CanWrite)
        {
            throw new NotSupportedException("Writing is only supported in compression mode.");
        }

        using Stream compressionStream = CompressionType switch
        {
            CompressionType.RefPack => new RefPackStream(_baseStream, CompressionMode.Compress, leaveOpen: true),
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

        // Write the header (magic bytes + uncompressed size).
        var magicBytes = GetMagicBytes(CompressionType);
        using (BinaryWriter writer = new(compressionStream, LegacyEncodings.Ansi, leaveOpen: true))
        {
            writer.Write(magicBytes);
            writer.Write(count);
        }

        // The first 8 bytes are used for the header, so we need to adjust the buffer accordingly.
        compressionStream.Write(buffer, offset + 8, count - 8);
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

    private static byte[] GetMagicBytes(CompressionType compressionType) =>
        compressionType switch
        {
            CompressionType.None => [],
            CompressionType.RefPack => "EAR\0"u8.ToArray(),
            CompressionType.NoxLzh => "NOX\0"u8.ToArray(),
            CompressionType.ZLib1 => "ZL1\0"u8.ToArray(),
            CompressionType.ZLib2 => "ZL2\0"u8.ToArray(),
            CompressionType.ZLib3 => "ZL3\0"u8.ToArray(),
            CompressionType.ZLib4 => "ZL4\0"u8.ToArray(),
            CompressionType.ZLib5 => "ZL5\0"u8.ToArray(),
            CompressionType.ZLib6 => "ZL6\0"u8.ToArray(),
            CompressionType.ZLib7 => "ZL7\0"u8.ToArray(),
            CompressionType.ZLib8 => "ZL8\0"u8.ToArray(),
            CompressionType.ZLib9 => "ZL9\0"u8.ToArray(),
            CompressionType.BinaryTree => "EAB\0"u8.ToArray(),
            CompressionType.HuffmanWithRunlength => "EAH\0"u8.ToArray(),
            _ => throw new InvalidOperationException("Unsupported compression type for magic bytes."),
        };
}
