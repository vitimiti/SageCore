// -----------------------------------------------------------------------
// <copyright file="HuffStat.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal class HuffStat
{
    public short[] Stat { get; } = new short[Constants.HuffSymbolsCount];

    internal int MakeSortedTemp(Span<HuffStatTemp> temps)
    {
        var total = 0;
        for (var i = 0; i < Constants.HuffSymbolsCount; i++)
        {
            temps[i] = new HuffStatTemp { I = (short)i, N = Stat[i] };
            total += Stat[i];
            Stat[i] = (short)(Stat[i] >> 1);
        }

        ShellSort(temps, Constants.HuffSymbolsCount - 1);
        return total;
    }

    private static void ShellSort(Span<HuffStatTemp> temps, int n)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(13, n / 9);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(40, n / 9);
        for (var i = 40; i > 0; i /= 3)
        {
            for (var j = i + 1; j <= n; j++)
            {
                HuffStatTemp v = temps[j];
                var k = j;
                while ((k > i) && (v < temps[k - i]))
                {
                    temps[k] = temps[k - i];
                    k -= i;
                }

                temps[k] = v;
            }
        }
    }
}
