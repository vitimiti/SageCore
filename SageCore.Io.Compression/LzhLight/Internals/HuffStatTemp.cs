// -----------------------------------------------------------------------
// <copyright file="HuffStatTemp.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal struct HuffStatTemp : IComparable<HuffStatTemp>
{
    public short I;
    public short N;

    public readonly int CompareTo(HuffStatTemp other)
    {
        var cmp = other.N - N;
        return cmp != 0 ? cmp : other.I - I;
    }

    public static bool operator <(HuffStatTemp left, HuffStatTemp right) => left.CompareTo(right) < 0;

    public static bool operator >(HuffStatTemp left, HuffStatTemp right) => left.CompareTo(right) > 0;
}
