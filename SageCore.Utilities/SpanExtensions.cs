// -----------------------------------------------------------------------
// <copyright file="SpanExtensions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Buffers.Binary;

namespace SageCore.Utilities;

/// <summary>
/// Provides extension methods for <see cref="Span{T}"/> to write integers in big-endian format.
/// </summary>
public static class SpanExtensions
{
    /// <summary>
    /// Writes a 16-bit unsigned integer to the span in big-endian format.
    /// </summary>
    /// <param name="span">The span to write to.</param>
    /// <param name="value">The 16-bit unsigned integer to write.</param>
    /// <remarks>
    /// This method uses <see cref="BinaryPrimitives.WriteUInt16BigEndian(Span{byte}, ushort)"/> for efficient writing.
    /// </remarks>
    public static void WriteUInt16BigEndian(this Span<byte> span, ushort value) =>
        BinaryPrimitives.WriteUInt16BigEndian(span, value);

    /// <summary>
    /// Writes a 24-bit unsigned integer to the span in big-endian format.
    /// </summary>
    /// <param name="span">The span to write to.</param>
    /// <param name="value">The 24-bit unsigned integer to write.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is greater than 0xFFFFFF.</exception>
    /// <remarks>
    /// Since .NET does not have a built-in method for writing 24-bit integers, this method manually writes the bytes in big-endian order.
    /// </remarks>
    public static void WriteUInt24BigEndian(this Span<byte> span, uint value)
    {
        if (value > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be a 24-bit unsigned integer.");
        }

        span[0] = (byte)((value >> 16) & 0xFF);
        span[1] = (byte)((value >> 8) & 0xFF);
        span[2] = (byte)(value & 0xFF);
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer to the span in big-endian format.
    /// </summary>
    /// <param name="span">The span to write to.</param>
    /// <param name="value">The 32-bit unsigned integer to write.</param>
    /// <remarks>
    /// This method uses <see cref="BinaryPrimitives.WriteUInt32BigEndian(Span{byte}, uint)"/> for efficient writing.
    /// </remarks>
    public static void WriteUInt32BigEndian(this Span<byte> span, uint value) =>
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
}
