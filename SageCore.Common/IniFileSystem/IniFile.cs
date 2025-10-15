// -----------------------------------------------------------------------
// <copyright file="IniFile.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Common.IniFileSystem;

/// <summary>
/// Represents an INI file and provides constants related to INI file handling.
/// </summary>
public class Ini
{
    /// <summary>
    /// A delegate for parsing a field in an INI file.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance being parsed.</typeparam>
    /// <typeparam name="TStore">The type of the store used during parsing.</typeparam>
    /// <param name="ini">The INI file being parsed.</param>
    /// <param name="instance">The instance being parsed.</param>
    /// <param name="store">The store used during parsing.</param>
    /// <returns>The parsed instance.</returns>
    public delegate TInstance FieldParse<TInstance, TStore>(Ini ini, TInstance instance, TStore store)
        where TStore : class; // ref objects to be able to modify them in place

    /// <summary>
    /// The maximum number of characters allowed per line in an INI file.
    /// </summary>
    public const int MaxCharsPerLine = 0x04_04;

    /// <summary>
    /// A lookup dictionary for list record types in INI files.
    /// </summary>
    public static readonly Dictionary<string, int> LookupListRecord = [];

    public static void InitFromMultiProcess() => throw new NotImplementedException();
}
