// -----------------------------------------------------------------------
// <copyright file="Region2D.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Models;

/// <summary>
/// Represents a 2D region defined by two coordinates: Lo (lower-left) and Hi (upper-right).
/// </summary>
/// <param name="Lo">The lower-left coordinate of the region.</param>
/// <param name="Hi">The upper-right coordinate of the region.</param>
public record Region2D(Coord2D Lo, Coord2D Hi)
{
    /// <summary>
    /// Gets the width of the region.
    /// </summary>
    public float Width => Hi.X - Lo.X;

    /// <summary>
    /// Gets the height of the region.
    /// </summary>
    public float Height => Hi.Y - Lo.Y;
}
