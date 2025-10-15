// -----------------------------------------------------------------------
// <copyright file="IniLoadType.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Common.IniFileSystem;

/// <summary>
/// Specifies how INI files should be loaded and merged.
/// </summary>
public enum IniLoadType
{
    /// <summary>
    /// The INI file is invalid or could not be loaded.
    /// </summary>
    Invalid,

    /// <summary>
    /// The INI file should overwrite any existing settings.
    /// </summary>
    Overwrite,

    /// <summary>
    /// The INI file should create overrides for existing settings.
    /// </summary>
    CreateOverrides,

    /// <summary>
    /// The INI file is part of a multifile set and should be merged with others.
    /// </summary>
    Multifile,
}
