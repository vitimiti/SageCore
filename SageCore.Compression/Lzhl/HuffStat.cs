// -----------------------------------------------------------------------
// <copyright file="HuffStat.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Compression.Lzhl;

internal class HuffStat()
{
    public short[] Stat { get; } = new short[Constants.HuffSymbolsCount];

    public int MakeSortedTempStats(Span<HuffTempStat> tempStats)
    {
        var total = 0;
        for (var i = 0; i < Constants.HuffSymbolsCount; i++)
        {
            tempStats[i].I = (short)i;
            tempStats[i].N = Stat[i];

            total += Stat[i];
            Stat[i] = (short)(Stat[i] >> 1);
        }

        ShellSort(tempStats, Constants.HuffSymbolsCount - 1);
        return total;
    }

    private static void ShellSort(Span<HuffTempStat> tempStats, int n)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(13, n / 9);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(40, n / 9);

        for (var i = 40; i > 0; i /= 3) // 40, 13, 4, 1
        {
            for (var j = i + 1; j <= n; j++)
            {
                HuffTempStat value = tempStats[j];
                var k = j;
                while ((k > i) && (value < tempStats[k - i]))
                {
                    tempStats[k] = tempStats[k - i];
                    k -= i;
                }

                tempStats[k] = value;
            }
        }
    }
}
