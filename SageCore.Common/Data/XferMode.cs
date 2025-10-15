// -----------------------------------------------------------------------
// <copyright file="XferMode.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Common.Data;

/// <summary>
/// Specifies the mode of transfer operation.
/// </summary>
public enum XferMode
{
    /// <summary>
    /// The transfer mode is invalid.
    /// </summary>
    Invalid,

    /// <summary>
    /// The transfer operation is a save operation.
    /// </summary>
    Save,

    /// <summary>
    /// The transfer operation is a load operation.
    /// </summary>
    Load,

    /// <summary>
    /// The transfer operation is a cyclic redundancy check (CRC) operation.
    /// </summary>
    Crc,
}
