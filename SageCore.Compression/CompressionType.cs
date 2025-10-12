// -----------------------------------------------------------------------
// <copyright file="CompressionType.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Compression;

/// <summary>
/// Specifies the type of compression to be used.
/// </summary>
public enum CompressionType
{
    /// <summary>
    /// No compression.
    /// </summary>
    None,

    /// <summary>
    /// RefPack compression.
    /// </summary>
    /// <remarks>
    /// This is the preferred compression type for SageCore assets.
    /// </remarks>
    RefPack,

    /// <summary>
    /// NoxLzh compression.
    /// </summary>
    NoxLzh,

    /// <summary>
    /// ZLib compression with level 1.
    /// </summary>
    ZLib1,

    /// <summary>
    /// ZLib compression with level 2.
    /// </summary>
    ZLib2,

    /// <summary>
    /// ZLib compression with level 3.
    /// </summary>
    ZLib3,

    /// <summary>
    /// ZLib compression with level 4.
    /// </summary>
    ZLib4,

    /// <summary>
    /// ZLib compression with level 5.
    /// </summary>
    ZLib5,

    /// <summary>
    /// ZLib compression with level 6.
    /// </summary>
    ZLib6,

    /// <summary>
    /// ZLib compression with level 7.
    /// </summary>
    ZLib7,

    /// <summary>
    /// ZLib compression with level 8.
    /// </summary>
    ZLib8,

    /// <summary>
    /// ZLib compression with level 9.
    /// </summary>
    ZLib9,

    /// <summary>
    /// Binary tree compression.
    /// </summary>
    BinaryTree,

    /// <summary>
    /// Huffman compression with run-length encoding.
    /// </summary>
    HuffmanWithRunLength,
}
