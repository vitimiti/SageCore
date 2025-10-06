// -----------------------------------------------------------------------
// <copyright file="EncoderStat.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Io.Compression.LzhLight.Internals;

internal class EncoderStat : HuffStat
{
    public int NextStat { get; private set; } = Constants.HuffRecalculateLength;

    public Symbol[] Symbols { get; } =
    [
        new() { BitsCount = 7, Code = 0x0014 },
        new() { BitsCount = 8, Code = 0x0030 },
        new() { BitsCount = 8, Code = 0x0031 },
        new() { BitsCount = 8, Code = 0x0032 },
        new() { BitsCount = 8, Code = 0x0033 },
        new() { BitsCount = 8, Code = 0x0034 },
        new() { BitsCount = 8, Code = 0x0035 },
        new() { BitsCount = 8, Code = 0x0036 },
        new() { BitsCount = 8, Code = 0x0037 },
        new() { BitsCount = 8, Code = 0x0038 },
        new() { BitsCount = 8, Code = 0x0039 },
        new() { BitsCount = 8, Code = 0x003A },
        new() { BitsCount = 8, Code = 0x003B },
        new() { BitsCount = 8, Code = 0x003C },
        new() { BitsCount = 8, Code = 0x003D },
        new() { BitsCount = 8, Code = 0x003E },
        new() { BitsCount = 8, Code = 0x003F },
        new() { BitsCount = 8, Code = 0x0040 },
        new() { BitsCount = 8, Code = 0x0041 },
        new() { BitsCount = 8, Code = 0x0042 },
        new() { BitsCount = 8, Code = 0x0043 },
        new() { BitsCount = 8, Code = 0x0044 },
        new() { BitsCount = 8, Code = 0x0045 },
        new() { BitsCount = 8, Code = 0x0046 },
        new() { BitsCount = 8, Code = 0x0047 },
        new() { BitsCount = 8, Code = 0x0048 },
        new() { BitsCount = 8, Code = 0x0049 },
        new() { BitsCount = 8, Code = 0x004A },
        new() { BitsCount = 8, Code = 0x004B },
        new() { BitsCount = 8, Code = 0x004C },
        new() { BitsCount = 8, Code = 0x004D },
        new() { BitsCount = 8, Code = 0x004E },
        new() { BitsCount = 7, Code = 0x0015 },
        new() { BitsCount = 8, Code = 0x004F },
        new() { BitsCount = 8, Code = 0x0050 },
        new() { BitsCount = 8, Code = 0x0051 },
        new() { BitsCount = 8, Code = 0x0052 },
        new() { BitsCount = 8, Code = 0x0053 },
        new() { BitsCount = 8, Code = 0x0054 },
        new() { BitsCount = 8, Code = 0x0055 },
        new() { BitsCount = 8, Code = 0x0056 },
        new() { BitsCount = 8, Code = 0x0057 },
        new() { BitsCount = 8, Code = 0x0058 },
        new() { BitsCount = 8, Code = 0x0059 },
        new() { BitsCount = 8, Code = 0x005A },
        new() { BitsCount = 8, Code = 0x005B },
        new() { BitsCount = 8, Code = 0x005C },
        new() { BitsCount = 8, Code = 0x005D },
        new() { BitsCount = 7, Code = 0x0016 },
        new() { BitsCount = 8, Code = 0x005E },
        new() { BitsCount = 8, Code = 0x005F },
        new() { BitsCount = 8, Code = 0x0060 },
        new() { BitsCount = 8, Code = 0x0061 },
        new() { BitsCount = 8, Code = 0x0062 },
        new() { BitsCount = 8, Code = 0x0063 },
        new() { BitsCount = 8, Code = 0x0064 },
        new() { BitsCount = 8, Code = 0x0065 },
        new() { BitsCount = 8, Code = 0x0066 },
        new() { BitsCount = 8, Code = 0x0067 },
        new() { BitsCount = 8, Code = 0x0068 },
        new() { BitsCount = 8, Code = 0x0069 },
        new() { BitsCount = 8, Code = 0x006A },
        new() { BitsCount = 8, Code = 0x006B },
        new() { BitsCount = 8, Code = 0x006C },
        new() { BitsCount = 8, Code = 0x006D },
        new() { BitsCount = 8, Code = 0x006E },
        new() { BitsCount = 8, Code = 0x006F },
        new() { BitsCount = 8, Code = 0x0070 },
        new() { BitsCount = 8, Code = 0x0071 },
        new() { BitsCount = 8, Code = 0x0072 },
        new() { BitsCount = 8, Code = 0x0073 },
        new() { BitsCount = 8, Code = 0x0074 },
        new() { BitsCount = 8, Code = 0x0075 },
        new() { BitsCount = 8, Code = 0x0076 },
        new() { BitsCount = 8, Code = 0x0077 },
        new() { BitsCount = 8, Code = 0x0078 },
        new() { BitsCount = 8, Code = 0x0079 },
        new() { BitsCount = 8, Code = 0x007A },
        new() { BitsCount = 8, Code = 0x007B },
        new() { BitsCount = 8, Code = 0x007C },
        new() { BitsCount = 8, Code = 0x007D },
        new() { BitsCount = 8, Code = 0x007E },
        new() { BitsCount = 8, Code = 0x007F },
        new() { BitsCount = 8, Code = 0x0080 },
        new() { BitsCount = 8, Code = 0x0081 },
        new() { BitsCount = 8, Code = 0x0082 },
        new() { BitsCount = 8, Code = 0x0083 },
        new() { BitsCount = 8, Code = 0x0084 },
        new() { BitsCount = 8, Code = 0x0085 },
        new() { BitsCount = 8, Code = 0x0086 },
        new() { BitsCount = 8, Code = 0x0087 },
        new() { BitsCount = 8, Code = 0x0088 },
        new() { BitsCount = 8, Code = 0x0089 },
        new() { BitsCount = 8, Code = 0x008A },
        new() { BitsCount = 8, Code = 0x008B },
        new() { BitsCount = 8, Code = 0x008C },
        new() { BitsCount = 8, Code = 0x008D },
        new() { BitsCount = 8, Code = 0x008E },
        new() { BitsCount = 8, Code = 0x008F },
        new() { BitsCount = 8, Code = 0x0090 },
        new() { BitsCount = 8, Code = 0x0091 },
        new() { BitsCount = 8, Code = 0x0092 },
        new() { BitsCount = 8, Code = 0x0093 },
        new() { BitsCount = 8, Code = 0x0094 },
        new() { BitsCount = 8, Code = 0x0095 },
        new() { BitsCount = 8, Code = 0x0096 },
        new() { BitsCount = 8, Code = 0x0097 },
        new() { BitsCount = 8, Code = 0x0098 },
        new() { BitsCount = 8, Code = 0x0099 },
        new() { BitsCount = 8, Code = 0x009A },
        new() { BitsCount = 8, Code = 0x009B },
        new() { BitsCount = 8, Code = 0x009C },
        new() { BitsCount = 8, Code = 0x009D },
        new() { BitsCount = 8, Code = 0x009E },
        new() { BitsCount = 8, Code = 0x009F },
        new() { BitsCount = 8, Code = 0x00A0 },
        new() { BitsCount = 8, Code = 0x00A1 },
        new() { BitsCount = 8, Code = 0x00A2 },
        new() { BitsCount = 8, Code = 0x00A3 },
        new() { BitsCount = 8, Code = 0x00A4 },
        new() { BitsCount = 8, Code = 0x00A5 },
        new() { BitsCount = 8, Code = 0x00A6 },
        new() { BitsCount = 8, Code = 0x00A7 },
        new() { BitsCount = 8, Code = 0x00A8 },
        new() { BitsCount = 8, Code = 0x00A9 },
        new() { BitsCount = 8, Code = 0x00AA },
        new() { BitsCount = 8, Code = 0x00AB },
        new() { BitsCount = 8, Code = 0x00AC },
        new() { BitsCount = 8, Code = 0x00AD },
        new() { BitsCount = 8, Code = 0x00AE },
        new() { BitsCount = 8, Code = 0x00AF },
        new() { BitsCount = 8, Code = 0x00B0 },
        new() { BitsCount = 8, Code = 0x00B1 },
        new() { BitsCount = 8, Code = 0x00B2 },
        new() { BitsCount = 8, Code = 0x00B3 },
        new() { BitsCount = 8, Code = 0x00B4 },
        new() { BitsCount = 8, Code = 0x00B5 },
        new() { BitsCount = 8, Code = 0x00B6 },
        new() { BitsCount = 8, Code = 0x00B7 },
        new() { BitsCount = 8, Code = 0x00B8 },
        new() { BitsCount = 8, Code = 0x00B9 },
        new() { BitsCount = 8, Code = 0x00BA },
        new() { BitsCount = 8, Code = 0x00BB },
        new() { BitsCount = 8, Code = 0x00BC },
        new() { BitsCount = 8, Code = 0x00BD },
        new() { BitsCount = 8, Code = 0x00BE },
        new() { BitsCount = 8, Code = 0x00BF },
        new() { BitsCount = 8, Code = 0x00C0 },
        new() { BitsCount = 8, Code = 0x00C1 },
        new() { BitsCount = 8, Code = 0x00C2 },
        new() { BitsCount = 8, Code = 0x00C3 },
        new() { BitsCount = 8, Code = 0x00C4 },
        new() { BitsCount = 8, Code = 0x00C5 },
        new() { BitsCount = 8, Code = 0x00C6 },
        new() { BitsCount = 8, Code = 0x00C7 },
        new() { BitsCount = 8, Code = 0x00C8 },
        new() { BitsCount = 8, Code = 0x00C9 },
        new() { BitsCount = 8, Code = 0x00CA },
        new() { BitsCount = 8, Code = 0x00CB },
        new() { BitsCount = 8, Code = 0x00CC },
        new() { BitsCount = 8, Code = 0x00CD },
        new() { BitsCount = 8, Code = 0x00CE },
        new() { BitsCount = 8, Code = 0x00CF },
        new() { BitsCount = 9, Code = 0x01A0 },
        new() { BitsCount = 9, Code = 0x01A1 },
        new() { BitsCount = 9, Code = 0x01A2 },
        new() { BitsCount = 9, Code = 0x01A3 },
        new() { BitsCount = 9, Code = 0x01A4 },
        new() { BitsCount = 9, Code = 0x01A5 },
        new() { BitsCount = 9, Code = 0x01A6 },
        new() { BitsCount = 9, Code = 0x01A7 },
        new() { BitsCount = 9, Code = 0x01A8 },
        new() { BitsCount = 9, Code = 0x01A9 },
        new() { BitsCount = 9, Code = 0x01AA },
        new() { BitsCount = 9, Code = 0x01AB },
        new() { BitsCount = 9, Code = 0x01AC },
        new() { BitsCount = 9, Code = 0x01AD },
        new() { BitsCount = 9, Code = 0x01AE },
        new() { BitsCount = 9, Code = 0x01AF },
        new() { BitsCount = 9, Code = 0x01B0 },
        new() { BitsCount = 9, Code = 0x01B1 },
        new() { BitsCount = 9, Code = 0x01B2 },
        new() { BitsCount = 9, Code = 0x01B3 },
        new() { BitsCount = 9, Code = 0x01B4 },
        new() { BitsCount = 9, Code = 0x01B5 },
        new() { BitsCount = 9, Code = 0x01B6 },
        new() { BitsCount = 9, Code = 0x01B7 },
        new() { BitsCount = 9, Code = 0x01B8 },
        new() { BitsCount = 9, Code = 0x01B9 },
        new() { BitsCount = 9, Code = 0x01BA },
        new() { BitsCount = 9, Code = 0x01BB },
        new() { BitsCount = 9, Code = 0x01BC },
        new() { BitsCount = 9, Code = 0x01BD },
        new() { BitsCount = 9, Code = 0x01BE },
        new() { BitsCount = 9, Code = 0x01BF },
        new() { BitsCount = 9, Code = 0x01C0 },
        new() { BitsCount = 9, Code = 0x01C1 },
        new() { BitsCount = 9, Code = 0x01C2 },
        new() { BitsCount = 9, Code = 0x01C3 },
        new() { BitsCount = 9, Code = 0x01C4 },
        new() { BitsCount = 9, Code = 0x01C5 },
        new() { BitsCount = 9, Code = 0x01C6 },
        new() { BitsCount = 9, Code = 0x01C7 },
        new() { BitsCount = 9, Code = 0x01C8 },
        new() { BitsCount = 9, Code = 0x01C9 },
        new() { BitsCount = 9, Code = 0x01CA },
        new() { BitsCount = 9, Code = 0x01CB },
        new() { BitsCount = 9, Code = 0x01CC },
        new() { BitsCount = 9, Code = 0x01CD },
        new() { BitsCount = 9, Code = 0x01CE },
        new() { BitsCount = 9, Code = 0x01CF },
        new() { BitsCount = 9, Code = 0x01D0 },
        new() { BitsCount = 9, Code = 0x01D1 },
        new() { BitsCount = 9, Code = 0x01D2 },
        new() { BitsCount = 9, Code = 0x01D3 },
        new() { BitsCount = 9, Code = 0x01D4 },
        new() { BitsCount = 9, Code = 0x01D5 },
        new() { BitsCount = 9, Code = 0x01D6 },
        new() { BitsCount = 9, Code = 0x01D7 },
        new() { BitsCount = 9, Code = 0x01D8 },
        new() { BitsCount = 9, Code = 0x01D9 },
        new() { BitsCount = 9, Code = 0x01DA },
        new() { BitsCount = 9, Code = 0x01DB },
        new() { BitsCount = 9, Code = 0x01DC },
        new() { BitsCount = 9, Code = 0x01DD },
        new() { BitsCount = 9, Code = 0x01DE },
        new() { BitsCount = 9, Code = 0x01DF },
        new() { BitsCount = 9, Code = 0x01E0 },
        new() { BitsCount = 9, Code = 0x01E1 },
        new() { BitsCount = 9, Code = 0x01E2 },
        new() { BitsCount = 9, Code = 0x01E3 },
        new() { BitsCount = 9, Code = 0x01E4 },
        new() { BitsCount = 9, Code = 0x01E5 },
        new() { BitsCount = 9, Code = 0x01E6 },
        new() { BitsCount = 9, Code = 0x01E7 },
        new() { BitsCount = 9, Code = 0x01E8 },
        new() { BitsCount = 9, Code = 0x01E9 },
        new() { BitsCount = 9, Code = 0x01EA },
        new() { BitsCount = 9, Code = 0x01EB },
        new() { BitsCount = 9, Code = 0x01EC },
        new() { BitsCount = 9, Code = 0x01ED },
        new() { BitsCount = 9, Code = 0x01EE },
        new() { BitsCount = 9, Code = 0x01EF },
        new() { BitsCount = 9, Code = 0x01F0 },
        new() { BitsCount = 9, Code = 0x01F1 },
        new() { BitsCount = 9, Code = 0x01F2 },
        new() { BitsCount = 9, Code = 0x01F3 },
        new() { BitsCount = 9, Code = 0x01F4 },
        new() { BitsCount = 9, Code = 0x01F5 },
        new() { BitsCount = 9, Code = 0x01F6 },
        new() { BitsCount = 9, Code = 0x01F7 },
        new() { BitsCount = 9, Code = 0x01F8 },
        new() { BitsCount = 9, Code = 0x01F9 },
        new() { BitsCount = 9, Code = 0x01FA },
        new() { BitsCount = 9, Code = 0x01FB },
        new() { BitsCount = 7, Code = 0x0017 },
        new() { BitsCount = 6, Code = 0x0000 },
        new() { BitsCount = 6, Code = 0x0001 },
        new() { BitsCount = 6, Code = 0x0002 },
        new() { BitsCount = 6, Code = 0x0003 },
        new() { BitsCount = 7, Code = 0x0008 },
        new() { BitsCount = 7, Code = 0x0009 },
        new() { BitsCount = 7, Code = 0x000A },
        new() { BitsCount = 7, Code = 0x000B },
        new() { BitsCount = 7, Code = 0x000C },
        new() { BitsCount = 7, Code = 0x000D },
        new() { BitsCount = 7, Code = 0x000E },
        new() { BitsCount = 7, Code = 0x000F },
        new() { BitsCount = 7, Code = 0x0010 },
        new() { BitsCount = 7, Code = 0x0011 },
        new() { BitsCount = 7, Code = 0x0012 },
        new() { BitsCount = 7, Code = 0x0013 },
        new() { BitsCount = 9, Code = 0x01FC },
        new() { BitsCount = 9, Code = 0x01FD },
    ];

    public void CalcStat(Span<int> groups)
    {
        Span<HuffStatTemp> temps = stackalloc HuffStatTemp[Constants.HuffSymbolsCount];
        var total = MakeSortedTemp(temps);

        NextStat = Constants.HuffRecalculateLength;

        var pos = 0;
        var totalCount = 0;
        var nn = 0;
        for (var group = 0; group < 14; group++)
        {
            var avgGroup = (total - totalCount) / (16 - group);
            var i = 0;
            var n = 0;
            for (var bitsCount = 0; ; bitsCount++)
            {
                var over = 0;
                var itemsCount = 1 << bitsCount;

                if (pos + i + itemsCount > Constants.HuffSymbolsCount)
                {
                    itemsCount = Constants.HuffSymbolsCount - pos;
                    over = 1;
                }

                for (; i < itemsCount; i++)
                {
                    nn += temps[pos + i].N;
                }

                if (over != 0 || bitsCount >= 8 || nn >= avgGroup)
                {
                    if (bitsCount == 0 || float.Abs(n - avgGroup) < float.Abs(nn - avgGroup))
                    {
                        n = nn;
                    }
                    else
                    {
                        bitsCount--;
                    }

                    AddGroup(groups, group, bitsCount);
                    totalCount += n;
                    pos += 1 << bitsCount;
                    break;
                }
                else
                {
                    n = nn;
                }
            }
        }

        var bestBitsCount = 0;
        var bestBitsCount15 = 0;
        var best = 0x7F_FF_FF_FF;
        var left = 0;
        for (var i = pos; i < Constants.HuffSymbolsCount; i++)
        {
            left += temps[i].N;
        }

        var j = 0;
        nn = 0;
        var bitsCount15 = 0;
        for (var bitsCount = 0; ; bitsCount++)
        {
            var itemsCount = 1 << bitsCount;
            if (pos + j + itemsCount > Constants.HuffSymbolsCount)
            {
                break;
            }

            for (; j < itemsCount; j++)
            {
                nn += temps[pos + j].N;
            }

            var itemsCount15 = Constants.HuffSymbolsCount - (pos + j);
            for (bitsCount15 = 0; ; bitsCount15++)
            {
                if (1 << bitsCount15 >= itemsCount15)
                {
                    break;
                }
            }

            if (left < nn)
            {
                throw new InvalidOperationException("Internal error in CalcStat");
            }

            if (bitsCount <= 8 && bitsCount15 <= 8)
            {
                var n = (nn * bitsCount) + ((left - nn) * bitsCount15);
                if (n < best)
                {
                    best = n;
                    bestBitsCount = bitsCount;
                    bestBitsCount15 = bitsCount15;
                }
                else
                {
                    break; // PERF optimization
                }
            }
        }

        var pos15 = pos + (1 << bestBitsCount);
        AddGroup(groups, 14, bestBitsCount);
        AddGroup(groups, 15, bestBitsCount15);

        pos = 0;
        for (var k = 0; k < 16; k++)
        {
            var bitsCount = groups[k];
            var itemsCount = 1 << bitsCount;
            var maxL = int.Min(itemsCount, Constants.HuffSymbolsCount - pos);
            for (var l = 0; l < maxL; l++)
            {
                var symbol = temps[pos + l].I;
                Symbols[symbol].BitsCount = (short)(bitsCount + 4);
                Symbols[symbol].Code = (ushort)((k << bitsCount) | l);
            }

            pos += 1 << bitsCount;
        }
    }

    private static void AddGroup(Span<int> groups, int group, int bitsCount)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bitsCount, 8);

        // Bubble sort
        int i;
        for (i = group; i > 0 && bitsCount < groups[i - 1]; i--)
        {
            groups[i] = groups[i - 1];
        }

        groups[i] = bitsCount;
    }

    public struct Symbol
    {
        public short BitsCount;
        public ushort Code;
    }
}
