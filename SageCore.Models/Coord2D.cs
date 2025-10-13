// -----------------------------------------------------------------------
// <copyright file="Coord2D.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Models;

/// <summary>
/// Represents a 2D coordinate with X and Y components.
/// </summary>
/// <param name="x">The X component of the coordinate.</param>
/// <param name="y">The Y component of the coordinate.</param>
public class Coord2D(float x, float y)
{
    /// <summary>
    /// Gets or sets the X component of the coordinate.
    /// </summary>
    public float X { get; set; } = x;

    /// <summary>
    /// Gets or sets the Y component of the coordinate.
    /// </summary>
    public float Y { get; set; } = y;

    /// <summary>
    /// Gets the length (magnitude) of the coordinate vector.
    /// </summary>
    public float Length => float.Sqrt(float.Pow(X, 2) + float.Pow(Y, 2));

    /// <summary>
    /// Normalizes the coordinate vector to have a length of 1.
    /// </summary>
    public void Normalize()
    {
        var length = Length;
        if (length > 0)
        {
            X /= length;
            Y /= length;
        }
    }

    /// <summary>
    /// Converts the coordinate vector to an angle in radians.
    /// </summary>
    /// <returns>The angle in radians.</returns>
    /// <remarks>
    /// This method uses the arccosine function to compute the angle.
    /// </remarks>
    public float ToAngle()
    {
        var length = Length;
        if (float.Abs(length) < float.Epsilon)
        {
            return 0F;
        }

        var c = float.Clamp(X / length, -1F, 1F);

        return Y < 0F ? -float.Acos(c) : float.Acos(c);
    }
}
