// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Compression.Lzhl;

internal static class Constants
{
    public const int HuffSymbolsCount = 256 + 16 + 2; // 256 literals + 16 length codes + 2 special codes
    public const int HuffRecalcLength = 0x10_00;
    public const int BufferBits = 16;
    public const int BufferSize = 1 << BufferBits;
    public const int BufferMask = BufferSize - 1;
    public const int TableBits = 15;
    public const int TableSize = 1 << TableBits;
    public const int SkipHash = 0x04_00;
    public const int Match = 5;
    public const int HashShift = 5;
    public const int HashMask = TableSize - 1;
    public const int Min = 4;
}
