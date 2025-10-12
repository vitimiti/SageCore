// -----------------------------------------------------------------------
// <copyright file="ReadOnlySpanExtensions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Buffers.Binary;

namespace SageCore.Extensions;

/// <summary>
/// Extension methods for <see cref="ReadOnlySpan{T}"/>.
/// </summary>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// Reads a big-endian encoded <see cref="ushort"/> from the start of the given <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span to read from.</param>
    /// <returns>The <see cref="ushort"/> read from the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the span is too small to contain a <see cref="ushort"/>.</exception>
    /// <remarks>
    /// This method uses <see cref="BinaryPrimitives.ReadUInt16BigEndian(ReadOnlySpan{byte})"/>
    /// to read the value from the span.
    /// </remarks>
    public static ushort ReadUInt16BigEndian(this ReadOnlySpan<byte> span) =>
        span.Length < 2
            ? throw new ArgumentOutOfRangeException(nameof(span), $"Span is too small to read a {nameof(UInt16)}.")
            : BinaryPrimitives.ReadUInt16BigEndian(span);

    /// <summary>
    /// Reads a big-endian encoded 24-bit unsigned integer from the start of the given <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span to read from.</param>
    /// <returns>The 24-bit unsigned integer read from the span as a <see cref="uint"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the span is too small to contain a 24-bit unsigned integer.</exception>
    /// <remarks>
    /// Since .NET does not have a built-in type for 24-bit integers, this method returns the value as a <see cref="uint"/>.
    /// The value is constructed by reading three bytes from the span and combining them into a single <see cref="uint"/>.
    /// </remarks>
    public static uint ReadUInt24BigEndian(this ReadOnlySpan<byte> span) =>
        span.Length < 3
            ? throw new ArgumentOutOfRangeException(
                nameof(span),
                "Span is too small to read a 24-bit unsigned integer."
            )
            : (uint)((span[0] << 16) | (span[1] << 8) | span[2]);

    /// <summary>
    /// Reads a big-endian encoded <see cref="uint"/> from the start of the given <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="span">The span to read from.</param>
    /// <returns>The <see cref="uint"/> read from the span.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the span is too small to contain a <see cref="uint"/>.</exception>
    /// <remarks>
    /// This method uses <see cref="BinaryPrimitives.ReadUInt32BigEndian(ReadOnlySpan{byte})"/>
    /// to read the value from the span.
    /// </remarks>
    public static uint ReadUInt32BigEndian(this ReadOnlySpan<byte> span) =>
        span.Length < 4
            ? throw new ArgumentOutOfRangeException(nameof(span), $"Span is too small to read a {nameof(UInt32)}.")
            : BinaryPrimitives.ReadUInt32BigEndian(span);
}
