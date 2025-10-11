// -----------------------------------------------------------------------
// <copyright file="Common.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Compression.Lzhl;

internal static class Common
{
    public const int HuffSymbolsCount = 256 + 16 + 2; // 256 literals + 16 length codes + 2 special codes
}
