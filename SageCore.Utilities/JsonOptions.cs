// -----------------------------------------------------------------------
// <copyright file="JsonOptions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;

namespace SageCore.Utilities;

/// <summary>
/// Provides JSON serialization options for the SageCore application.
/// </summary>
public static class JsonOptions
{
    /// <summary>
    /// Gets the default JSON serializer options with indented formatting and camel case property naming.
    /// </summary>
    public static JsonSerializerOptions DefaultOptions =>
        new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}
