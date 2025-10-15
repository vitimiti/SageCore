// -----------------------------------------------------------------------
// <copyright file="IniFile.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Common.Data;

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
    /// A delegate for parsing a block in an INI file.
    /// </summary>
    /// <param name="ini">The INI file being parsed.</param>
    public delegate void BlockParse(Ini ini);

    /// <summary>
    /// A delegate for building multi-field processes in INI file parsing.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance being processed.</typeparam>
    /// <typeparam name="TStore">The type of the store used during processing.</typeparam>
    /// <param name="multiFieldParse">The multi-field parser to build upon.</param>
    public delegate void BuildMultiFieldProcess<TInstance, TStore>(
        MultiIniFieldParse<TInstance, TStore> multiFieldParse
    )
        where TStore : class;

    /// <summary>
    /// The maximum number of characters allowed per line in an INI file.
    /// </summary>
    public const int MaxCharsPerLine = 0x04_04;

    /// <summary>
    /// A lookup dictionary for list record types in INI files.
    /// </summary>
    public static readonly Dictionary<string, int> LookupListRecord = [];

    public static void InitFromMultiProcess() => throw new NotImplementedException();

    public void LoadDirectory(string directoryName, bool subDirectories, IniLoadType laodType, Xfer xfer) =>
        throw new NotImplementedException();
}
