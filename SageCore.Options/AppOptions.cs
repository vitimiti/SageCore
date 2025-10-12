// -----------------------------------------------------------------------
// <copyright file="AppOptions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;

namespace SageCore.Options;

/// <summary>
/// Represents application configuration options for the SageCore application.
/// </summary>
public class AppOptions
{
    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    /// <remarks>
    /// Default is "SageCore".
    /// </remarks>
    public string Name { get; set; } = "SageCore";

    /// <summary>
    /// Gets or sets the version of the application.
    /// </summary>
    /// <remarks>
    /// Default is the version of the entry assembly (or 1.0.0 if not found).
    /// </remarks>
    public Version Version { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0);

    /// <summary>
    /// Gets or sets the unique identifier of the application.
    /// </summary>
    /// <remarks>
    /// Default is "com.sagecore.app".
    /// </remarks>
    public string Identifier { get; set; } = "com.sagecore.app";

    /// <summary>
    /// Gets or sets the creator of the application.
    /// </summary>
    /// <remarks>
    /// Default is "SageCore Contributors".
    /// </remarks>
    public string Creator { get; set; } = "SageCore Contributors";

    /// <summary>
    /// Gets or sets the copyright information of the application.
    /// </summary>
    /// <remarks>
    /// Default is "2025 (c) SageCore Contributors. All rights reserved. Licensed under the MIT license.".
    /// </remarks>
    public string Copyright { get; set; } =
        "2025 (c) SageCore Contributors. All rights reserved. Licensed under the MIT license.";

    /// <summary>
    /// Gets or sets the URL of the application.
    /// </summary>
    /// <remarks>
    /// Default is "https://github.com/vitimiti/SageCore".
    /// </remarks>
    public Uri Url { get; set; } = new Uri("https://github.com/vitimiti/SageCore");

    /// <summary>
    /// Gets or sets the type of the application.
    /// </summary>
    /// <remarks>
    /// Default is "Game".
    /// </remarks>
    public string AppType { get; set; } = "Game";

    /// <summary>
    /// Returns a JSON string representation of the screen options.
    /// </summary>
    /// <returns>A formatted JSON string containing the screen options.</returns>
    public override string ToString() => JsonSerializer.Serialize(this, JsonOptions.DefaultOptions);
}
