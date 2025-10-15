// -----------------------------------------------------------------------
// <copyright file="FieldParse.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace SageCore.Common.IniFileSystem;

/// <summary>
/// Delegate for parsing a field in an INI file.
/// </summary>
/// <typeparam name="TInstance">The type of the instance being parsed.</typeparam>
/// <typeparam name="TStore">The type of the store used during parsing.</typeparam>
[DebuggerDisplay("Token = {Token}, Offset = {Offset}")]
public class FieldParse<TInstance, TStore>
    where TStore : class
{
    /// <summary>
    /// Gets or sets the token associated with this field.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the parsing function for this field.
    /// </summary>
    public Ini.FieldParse<TInstance, TStore>? Parse { get; set; }

    /// <summary>
    /// Gets or sets the offset for this field.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// Sets the properties of the FieldParse instance.
    /// </summary>
    /// <param name="token">The token associated with this field.</param>
    /// <param name="parse">The parsing function for this field.</param>
    /// <param name="offset">The offset for this field.</param>
    public void Set(string token, Ini.FieldParse<TInstance, TStore> parse, int offset)
    {
        Token = token;
        Parse = parse;
        Offset = offset;
    }
}
