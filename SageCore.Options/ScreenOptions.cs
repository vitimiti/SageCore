// -----------------------------------------------------------------------
// <copyright file="ScreenOptions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;
using SageCore.Utilities;

namespace SageCore.Options;

/// <summary>
/// Represents screen configuration options for the SageCore application.
/// </summary>
public class ScreenOptions
{
    /// <summary>
    /// Gets or sets the title of the application window.
    /// </summary>
    /// <remarks>
    /// Default is "SageCore Application".
    /// </remarks>
    public string Title { get; set; } = "SageCore Application";

    /// <summary>
    /// Gets or sets the width of the application window in pixels.
    /// </summary>
    /// <remarks>
    /// Default is 800.
    /// </remarks>
    public int Width { get; set; } = 800;

    /// <summary>
    /// Gets or sets the height of the application window in pixels.
    /// </summary>
    /// <remarks>
    /// Default is 600.
    /// </remarks>
    public int Height { get; set; } = 600;

    /// <summary>
    /// Gets or sets a value indicating whether the application should run in full screen mode.
    /// </summary>
    /// <remarks>
    /// Default is true.
    /// </remarks>
    public bool FullScreen { get; set; } = true;

    /// <summary>
    /// Returns a JSON string representation of the screen options.
    /// </summary>
    /// <returns>A formatted JSON string containing the screen options.</returns>
    public override string ToString() => JsonSerializer.Serialize(this, JsonOptions.DefaultOptions);
}
