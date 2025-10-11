// -----------------------------------------------------------------------
// <copyright file="HuffTempStat.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace SageCore.Compression.Lzhl;

[DebuggerDisplay("I={I}, N={N}")]
internal struct HuffTempStat : IComparable<HuffTempStat>
{
    public short I;
    public short N;

    public readonly int CompareTo(HuffTempStat other)
    {
        var cmp = other.N - N;
        return cmp != 0 ? cmp : other.I - I;
    }

    public static bool operator <(HuffTempStat left, HuffTempStat right) => left.CompareTo(right) < 0;

    public static bool operator >(HuffTempStat left, HuffTempStat right) => left.CompareTo(right) > 0;
}
