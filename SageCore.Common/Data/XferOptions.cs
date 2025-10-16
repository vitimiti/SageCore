// -----------------------------------------------------------------------
// <copyright file="XferOptions.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace SageCore.Common.Data;

/// <summary>
/// Specifies options for file transfer operations.
/// </summary>
[Flags]
[SuppressMessage(
    "Usage",
    "CA2217: Do not mark enums with FlagsAttribute",
    Justification = "This enum flags is intentionally empty to allow external definition."
)]
public enum XferOptions : uint
{
    /// <summary>
    /// No special options are set.
    /// </summary>
    None = 0,

    /// <summary>
    /// No post-processing should be applied after the transfer.
    /// </summary>
    NoPostProcessing = 1,

    /// <summary>
    /// All options are set.
    /// </summary>
    All = 0xFFFFFFFF,
}
