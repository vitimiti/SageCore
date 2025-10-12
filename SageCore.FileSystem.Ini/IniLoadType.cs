// -----------------------------------------------------------------------
// <copyright file="IniLoadType.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.FileSystem.Ini;

/// <summary>
/// Specifies the type of loading behavior when loading INI files.
/// </summary>
public enum IniLoadType
{
    /// <summary>
    /// Indicates an invalid load type.
    /// </summary>
    Invalid,

    /// <summary>
    /// Create new file or load <b>over</b> existing data instance.
    /// </summary>
    Overwrite,

    /// <summary>
    /// Create new file or load into <b>new</b> override data instance.
    /// </summary>
    CreateOverrides,

    /// <summary>
    /// Create new or continue loading into existing data instance.
    /// </summary>
    Multifile,
}
