// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Buffers.Binary;

namespace SageCore.Extensions;

/// <summary>
/// Provides extension methods for <see cref="BinaryReader"/>.
/// </summary>
public static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads a 16-bit unsigned integer from the current stream using big-endian encoding and advances the current position of the stream by two bytes.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
    /// <returns>A 16-bit unsigned integer read from the current stream.</returns>
    public static ushort ReadUInt16BigEndian(this BinaryReader reader)
    {
        const int bytesToread = 2;

        ArgumentNullException.ThrowIfNull(reader);
        if (!reader.BaseStream.CanRead)
        {
            throw new InvalidOperationException("The underlying stream does not support reading.");
        }

        if (reader.BaseStream.Length < bytesToread)
        {
            throw new EndOfStreamException("Not enough data to read a UInt16.");
        }

        var bytes = reader.ReadBytes(bytesToread);
        return BinaryPrimitives.ReadUInt16BigEndian(bytes);
    }

    /// <summary>
    /// Reads a 24-bit unsigned integer from the current stream using big-endian encoding and advances the current position of the stream by three bytes.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
    /// <returns>A 32-bit unsigned integer read from the current stream padded with zeros in big-endian order to make it 24 bits.</returns>
    public static uint ReadUInt24BigEndian(this BinaryReader reader)
    {
        const int bytesToread = 3;

        ArgumentNullException.ThrowIfNull(reader);
        if (!reader.BaseStream.CanRead)
        {
            throw new InvalidOperationException("The underlying stream does not support reading.");
        }

        if (reader.BaseStream.Length < bytesToread)
        {
            throw new EndOfStreamException("Not enough data to read a UInt24.");
        }

        var bytes = reader.ReadBytes(bytesToread);
        return (uint)((bytes[0] << 16) | (bytes[1] << 8) | bytes[2]);
    }

    /// <summary>
    /// Reads a 32-bit unsigned integer from the current stream using big-endian encoding and advances the current position of the stream by four bytes.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
    /// <returns>A 32-bit unsigned integer read from the current stream.</returns>
    public static uint ReadUInt32BigEndian(this BinaryReader reader)
    {
        const int bytesToread = 4;

        ArgumentNullException.ThrowIfNull(reader);
        if (!reader.BaseStream.CanRead)
        {
            throw new InvalidOperationException("The underlying stream does not support reading.");
        }

        if (reader.BaseStream.Length < bytesToread)
        {
            throw new EndOfStreamException("Not enough data to read a UInt32.");
        }

        var bytes = reader.ReadBytes(bytesToread);
        return BinaryPrimitives.ReadUInt32BigEndian(bytes);
    }
}
