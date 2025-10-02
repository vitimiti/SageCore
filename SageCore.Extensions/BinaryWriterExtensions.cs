// -----------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Buffers.Binary;

namespace SageCore.Extensions;

/// <summary>
/// Provides extension methods for <see cref="BinaryWriter"/>.
/// </summary>
public static class BinaryWriterExtensions
{
    /// <summary>
    /// Writes a 16-bit unsigned integer to the current stream using big-endian encoding and advances the current position of the stream by two bytes.
    /// </summary>
    /// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
    /// <param name="value">The 16-bit unsigned integer to write.</param>
    public static void WriteUInt16BigEndian(this BinaryWriter writer, ushort value)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (!writer.BaseStream.CanWrite)
        {
            throw new InvalidOperationException("The underlying stream does not support writing.");
        }

        Span<byte> bytes = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        writer.Write(bytes);
    }

    /// <summary>
    /// Writes a 24-bit unsigned integer to the current stream using big-endian encoding and advances the current position of the stream by three bytes.
    /// </summary>
    /// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
    /// <param name="value">The 32-bit unsigned integer to write. Only the lower 24 bits are used.</param>
    public static void WriteUInt24BigEndian(this BinaryWriter writer, uint value)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (!writer.BaseStream.CanWrite)
        {
            throw new InvalidOperationException("The underlying stream does not support writing.");
        }

        Span<byte> bytes = [(byte)(value >> 16), (byte)(value >> 8), (byte)unchecked(value)];
        writer.Write(bytes);
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer to the current stream using big-endian encoding and advances the current position of the stream by four bytes.
    /// </summary>
    /// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
    /// <param name="value">The 32-bit unsigned integer to write.</param>
    public static void WriteUInt32BigEndian(this BinaryWriter writer, uint value)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (!writer.BaseStream.CanWrite)
        {
            throw new InvalidOperationException("The underlying stream does not support writing.");
        }

        Span<byte> bytes = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        writer.Write(bytes);
    }
}
