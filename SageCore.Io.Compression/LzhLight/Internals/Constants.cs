// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal static class Constants
{
    public const int HuffSymbolsCount = 256 + 16 + 2;
    public const int HuffRecalculateLength = 0x10_00;
    public const int LzBufBits = 16;
    public const int LzBufSize = 1 << LzBufBits;
    public const int LzBufMask = LzBufSize - 1;
    public const int LzTableBits = 15;
    public const int LzTableSize = 1 << LzTableBits;
    public const int LzSkipHash = 0x04_00;
    public const int LzMatch = 5;
    public const int LzMin = 4;
}
