// -----------------------------------------------------------------------
// <copyright file="SageGameOptions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;
using SageCore.Options;
using SageCore.Utilities;

namespace SageCore;

/// <summary>
/// Represents the main configuration options for the SageCore game application.
/// </summary>
public class SageGameOptions
{
    /// <summary>
    /// Gets or sets the application options.
    /// </summary>
    /// <remarks>
    /// Default values are provided by the <see cref="AppOptions"/> class.
    /// </remarks>
    public AppOptions AppOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the screen configuration options.
    /// </summary>
    /// <remarks>
    /// Default values are provided by the <see cref="ScreenOptions"/> class.
    /// </remarks>
    public ScreenOptions ScreenOptions { get; set; } = new();

    /// <summary>
    /// Returns a JSON string representation of the screen options.
    /// </summary>
    /// <returns>A formatted JSON string containing the screen options.</returns>
    public override string ToString() => JsonSerializer.Serialize(this, JsonOptions.DefaultOptions);
}
