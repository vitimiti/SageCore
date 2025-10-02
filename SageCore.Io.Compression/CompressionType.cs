// -----------------------------------------------------------------------
// <copyright file="CompressionType.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression;

/// <summary>
/// Specifies the type of compression used in a <see cref="SageCompressionStream"/>.
/// </summary>
public enum CompressionType
{
    /// <summary>
    /// No compression is applied.
    /// </summary>
    None,

    /// <summary>
    /// RefPack compression.
    /// </summary>
    /// <remarks>
    /// This is the preferred compression method for most scenarios.
    /// </remarks>
    RefPack,

    /// <summary>
    /// NoxLzh compression.
    /// </summary>
    NoxLzh,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 1 compression.
    /// </remarks>
    ZLib1,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 2 compression.
    /// </remarks>
    ZLib2,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 3 compression.
    /// </remarks>
    ZLib3,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 4 compression.
    /// </remarks>
    ZLib4,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 5 compression.
    /// </remarks>
    ZLib5,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 6 compression.
    /// </remarks>
    ZLib6,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 7 compression.
    /// </remarks>
    ZLib7,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 8 compression.
    /// </remarks>
    ZLib8,

    /// <summary>
    /// ZLib compression with levels 1-9, where 1 is fastest with least compression and 9 is slowest with best compression.
    /// </summary>
    /// <remarks>
    /// This is the level 9 compression.
    /// </remarks>
    ZLib9,

    /// <summary>
    /// Binary tree compression.
    /// </summary>
    BinaryTree,

    /// <summary>
    /// Huffman with run-length encoding compression.
    /// </summary>
    HuffmanWithRunlength,
}
