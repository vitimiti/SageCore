// -----------------------------------------------------------------------
// <copyright file="LegacyEncodings.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

namespace SageCore.Extensions;

/// <summary>
/// Provides access to legacy text encodings.
/// </summary>
public static class LegacyEncodings
{
    static LegacyEncodings() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    /// <summary>
    /// Gets the ANSI encoding (Windows-1252).
    /// </summary>
    public static Encoding Ansi => Encoding.GetEncoding(1252);
}
