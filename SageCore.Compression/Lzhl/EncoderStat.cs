// -----------------------------------------------------------------------
// <copyright file="EncoderStat.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace SageCore.Compression.Lzhl;

internal sealed class EncoderStat : HuffStat
{
    public int NextStat { get; private set; }

    public Symbol[] SymbolTable { get; } =
    [
        new() { BitsCount = 7, Code = 0x00_14 },
        new() { BitsCount = 8, Code = 0x00_30 },
        new() { BitsCount = 8, Code = 0x00_31 },
        new() { BitsCount = 8, Code = 0x00_32 },
        new() { BitsCount = 8, Code = 0x00_33 },
        new() { BitsCount = 8, Code = 0x00_34 },
        new() { BitsCount = 8, Code = 0x00_35 },
        new() { BitsCount = 8, Code = 0x00_36 },
        new() { BitsCount = 8, Code = 0x00_37 },
        new() { BitsCount = 8, Code = 0x00_38 },
        new() { BitsCount = 8, Code = 0x00_39 },
        new() { BitsCount = 8, Code = 0x00_3A },
        new() { BitsCount = 8, Code = 0x00_3B },
        new() { BitsCount = 8, Code = 0x00_3C },
        new() { BitsCount = 8, Code = 0x00_3D },
        new() { BitsCount = 8, Code = 0x00_3E },
        new() { BitsCount = 8, Code = 0x00_3F },
        new() { BitsCount = 8, Code = 0x00_40 },
        new() { BitsCount = 8, Code = 0x00_41 },
        new() { BitsCount = 8, Code = 0x00_42 },
        new() { BitsCount = 8, Code = 0x00_43 },
        new() { BitsCount = 8, Code = 0x00_44 },
        new() { BitsCount = 8, Code = 0x00_45 },
        new() { BitsCount = 8, Code = 0x00_46 },
        new() { BitsCount = 8, Code = 0x00_47 },
        new() { BitsCount = 8, Code = 0x00_48 },
        new() { BitsCount = 8, Code = 0x00_49 },
        new() { BitsCount = 8, Code = 0x00_4A },
        new() { BitsCount = 8, Code = 0x00_4B },
        new() { BitsCount = 8, Code = 0x00_4C },
        new() { BitsCount = 8, Code = 0x00_4D },
        new() { BitsCount = 8, Code = 0x00_4E },
        new() { BitsCount = 7, Code = 0x00_15 },
        new() { BitsCount = 8, Code = 0x00_4F },
        new() { BitsCount = 8, Code = 0x00_50 },
        new() { BitsCount = 8, Code = 0x00_51 },
        new() { BitsCount = 8, Code = 0x00_52 },
        new() { BitsCount = 8, Code = 0x00_53 },
        new() { BitsCount = 8, Code = 0x00_54 },
        new() { BitsCount = 8, Code = 0x00_55 },
        new() { BitsCount = 8, Code = 0x00_56 },
        new() { BitsCount = 8, Code = 0x00_57 },
        new() { BitsCount = 8, Code = 0x00_58 },
        new() { BitsCount = 8, Code = 0x00_59 },
        new() { BitsCount = 8, Code = 0x00_5A },
        new() { BitsCount = 8, Code = 0x00_5B },
        new() { BitsCount = 8, Code = 0x00_5C },
        new() { BitsCount = 8, Code = 0x00_5D },
        new() { BitsCount = 7, Code = 0x00_16 },
        new() { BitsCount = 8, Code = 0x00_5E },
        new() { BitsCount = 8, Code = 0x00_5F },
        new() { BitsCount = 8, Code = 0x00_60 },
        new() { BitsCount = 8, Code = 0x00_61 },
        new() { BitsCount = 8, Code = 0x00_62 },
        new() { BitsCount = 8, Code = 0x00_63 },
        new() { BitsCount = 8, Code = 0x00_64 },
        new() { BitsCount = 8, Code = 0x00_65 },
        new() { BitsCount = 8, Code = 0x00_66 },
        new() { BitsCount = 8, Code = 0x00_67 },
        new() { BitsCount = 8, Code = 0x00_68 },
        new() { BitsCount = 8, Code = 0x00_69 },
        new() { BitsCount = 8, Code = 0x00_6A },
        new() { BitsCount = 8, Code = 0x00_6B },
        new() { BitsCount = 8, Code = 0x00_6C },
        new() { BitsCount = 8, Code = 0x00_6D },
        new() { BitsCount = 8, Code = 0x00_6E },
        new() { BitsCount = 8, Code = 0x00_6F },
        new() { BitsCount = 8, Code = 0x00_70 },
        new() { BitsCount = 8, Code = 0x00_71 },
        new() { BitsCount = 8, Code = 0x00_72 },
        new() { BitsCount = 8, Code = 0x00_73 },
        new() { BitsCount = 8, Code = 0x00_74 },
        new() { BitsCount = 8, Code = 0x00_75 },
        new() { BitsCount = 8, Code = 0x00_76 },
        new() { BitsCount = 8, Code = 0x00_77 },
        new() { BitsCount = 8, Code = 0x00_78 },
        new() { BitsCount = 8, Code = 0x00_79 },
        new() { BitsCount = 8, Code = 0x00_7A },
        new() { BitsCount = 8, Code = 0x00_7B },
        new() { BitsCount = 8, Code = 0x00_7C },
        new() { BitsCount = 8, Code = 0x00_7D },
        new() { BitsCount = 8, Code = 0x00_7E },
        new() { BitsCount = 8, Code = 0x00_7F },
        new() { BitsCount = 8, Code = 0x00_80 },
        new() { BitsCount = 8, Code = 0x00_81 },
        new() { BitsCount = 8, Code = 0x00_82 },
        new() { BitsCount = 8, Code = 0x00_83 },
        new() { BitsCount = 8, Code = 0x00_84 },
        new() { BitsCount = 8, Code = 0x00_85 },
        new() { BitsCount = 8, Code = 0x00_86 },
        new() { BitsCount = 8, Code = 0x00_87 },
        new() { BitsCount = 8, Code = 0x00_88 },
        new() { BitsCount = 8, Code = 0x00_89 },
        new() { BitsCount = 8, Code = 0x00_8A },
        new() { BitsCount = 8, Code = 0x00_8B },
        new() { BitsCount = 8, Code = 0x00_8C },
        new() { BitsCount = 8, Code = 0x00_8D },
        new() { BitsCount = 8, Code = 0x00_8E },
        new() { BitsCount = 8, Code = 0x00_8F },
        new() { BitsCount = 8, Code = 0x00_90 },
        new() { BitsCount = 8, Code = 0x00_91 },
        new() { BitsCount = 8, Code = 0x00_92 },
        new() { BitsCount = 8, Code = 0x00_93 },
        new() { BitsCount = 8, Code = 0x00_94 },
        new() { BitsCount = 8, Code = 0x00_95 },
        new() { BitsCount = 8, Code = 0x00_96 },
        new() { BitsCount = 8, Code = 0x00_97 },
        new() { BitsCount = 8, Code = 0x00_98 },
        new() { BitsCount = 8, Code = 0x00_99 },
        new() { BitsCount = 8, Code = 0x00_9A },
        new() { BitsCount = 8, Code = 0x00_9B },
        new() { BitsCount = 8, Code = 0x00_9C },
        new() { BitsCount = 8, Code = 0x00_9D },
        new() { BitsCount = 8, Code = 0x00_9E },
        new() { BitsCount = 8, Code = 0x00_9F },
        new() { BitsCount = 8, Code = 0x00_A0 },
        new() { BitsCount = 8, Code = 0x00_A1 },
        new() { BitsCount = 8, Code = 0x00_A2 },
        new() { BitsCount = 8, Code = 0x00_A3 },
        new() { BitsCount = 8, Code = 0x00_A4 },
        new() { BitsCount = 8, Code = 0x00_A5 },
        new() { BitsCount = 8, Code = 0x00_A6 },
        new() { BitsCount = 8, Code = 0x00_A7 },
        new() { BitsCount = 8, Code = 0x00_A8 },
        new() { BitsCount = 8, Code = 0x00_A9 },
        new() { BitsCount = 8, Code = 0x00_AA },
        new() { BitsCount = 8, Code = 0x00_AB },
        new() { BitsCount = 8, Code = 0x00_AC },
        new() { BitsCount = 8, Code = 0x00_AD },
        new() { BitsCount = 8, Code = 0x00_AE },
        new() { BitsCount = 8, Code = 0x00_AF },
        new() { BitsCount = 8, Code = 0x00_B0 },
        new() { BitsCount = 8, Code = 0x00_B1 },
        new() { BitsCount = 8, Code = 0x00_B2 },
        new() { BitsCount = 8, Code = 0x00_B3 },
        new() { BitsCount = 8, Code = 0x00_B4 },
        new() { BitsCount = 8, Code = 0x00_B5 },
        new() { BitsCount = 8, Code = 0x00_B6 },
        new() { BitsCount = 8, Code = 0x00_B7 },
        new() { BitsCount = 8, Code = 0x00_B8 },
        new() { BitsCount = 8, Code = 0x00_B9 },
        new() { BitsCount = 8, Code = 0x00_BA },
        new() { BitsCount = 8, Code = 0x00_BB },
        new() { BitsCount = 8, Code = 0x00_BC },
        new() { BitsCount = 8, Code = 0x00_BD },
        new() { BitsCount = 8, Code = 0x00_BE },
        new() { BitsCount = 8, Code = 0x00_BF },
        new() { BitsCount = 8, Code = 0x00_C0 },
        new() { BitsCount = 8, Code = 0x00_C1 },
        new() { BitsCount = 8, Code = 0x00_C2 },
        new() { BitsCount = 8, Code = 0x00_C3 },
        new() { BitsCount = 8, Code = 0x00_C4 },
        new() { BitsCount = 8, Code = 0x00_C5 },
        new() { BitsCount = 8, Code = 0x00_C6 },
        new() { BitsCount = 8, Code = 0x00_C7 },
        new() { BitsCount = 8, Code = 0x00_C8 },
        new() { BitsCount = 8, Code = 0x00_C9 },
        new() { BitsCount = 8, Code = 0x00_CA },
        new() { BitsCount = 8, Code = 0x00_CB },
        new() { BitsCount = 8, Code = 0x00_CC },
        new() { BitsCount = 8, Code = 0x00_CD },
        new() { BitsCount = 8, Code = 0x00_CE },
        new() { BitsCount = 8, Code = 0x00_CF },
        new() { BitsCount = 9, Code = 0x01_A0 },
        new() { BitsCount = 9, Code = 0x01_A1 },
        new() { BitsCount = 9, Code = 0x01_A2 },
        new() { BitsCount = 9, Code = 0x01_A3 },
        new() { BitsCount = 9, Code = 0x01_A4 },
        new() { BitsCount = 9, Code = 0x01_A5 },
        new() { BitsCount = 9, Code = 0x01_A6 },
        new() { BitsCount = 9, Code = 0x01_A7 },
        new() { BitsCount = 9, Code = 0x01_A8 },
        new() { BitsCount = 9, Code = 0x01_A9 },
        new() { BitsCount = 9, Code = 0x01_AA },
        new() { BitsCount = 9, Code = 0x01_AB },
        new() { BitsCount = 9, Code = 0x01_AC },
        new() { BitsCount = 9, Code = 0x01_AD },
        new() { BitsCount = 9, Code = 0x01_AE },
        new() { BitsCount = 9, Code = 0x01_AF },
        new() { BitsCount = 9, Code = 0x01_B0 },
        new() { BitsCount = 9, Code = 0x01_B1 },
        new() { BitsCount = 9, Code = 0x01_B2 },
        new() { BitsCount = 9, Code = 0x01_B3 },
        new() { BitsCount = 9, Code = 0x01_B4 },
        new() { BitsCount = 9, Code = 0x01_B5 },
        new() { BitsCount = 9, Code = 0x01_B6 },
        new() { BitsCount = 9, Code = 0x01_B7 },
        new() { BitsCount = 9, Code = 0x01_B8 },
        new() { BitsCount = 9, Code = 0x01_B9 },
        new() { BitsCount = 9, Code = 0x01_BA },
        new() { BitsCount = 9, Code = 0x01_BB },
        new() { BitsCount = 9, Code = 0x01_BC },
        new() { BitsCount = 9, Code = 0x01_BD },
        new() { BitsCount = 9, Code = 0x01_BE },
        new() { BitsCount = 9, Code = 0x01_BF },
        new() { BitsCount = 9, Code = 0x01_C0 },
        new() { BitsCount = 9, Code = 0x01_C1 },
        new() { BitsCount = 9, Code = 0x01_C2 },
        new() { BitsCount = 9, Code = 0x01_C3 },
        new() { BitsCount = 9, Code = 0x01_C4 },
        new() { BitsCount = 9, Code = 0x01_C5 },
        new() { BitsCount = 9, Code = 0x01_C6 },
        new() { BitsCount = 9, Code = 0x01_C7 },
        new() { BitsCount = 9, Code = 0x01_C8 },
        new() { BitsCount = 9, Code = 0x01_C9 },
        new() { BitsCount = 9, Code = 0x01_CA },
        new() { BitsCount = 9, Code = 0x01_CB },
        new() { BitsCount = 9, Code = 0x01_CC },
        new() { BitsCount = 9, Code = 0x01_CD },
        new() { BitsCount = 9, Code = 0x01_CE },
        new() { BitsCount = 9, Code = 0x01_CF },
        new() { BitsCount = 9, Code = 0x01_D0 },
        new() { BitsCount = 9, Code = 0x01_D1 },
        new() { BitsCount = 9, Code = 0x01_D2 },
        new() { BitsCount = 9, Code = 0x01_D3 },
        new() { BitsCount = 9, Code = 0x01_D4 },
        new() { BitsCount = 9, Code = 0x01_D5 },
        new() { BitsCount = 9, Code = 0x01_D6 },
        new() { BitsCount = 9, Code = 0x01_D7 },
        new() { BitsCount = 9, Code = 0x01_D8 },
        new() { BitsCount = 9, Code = 0x01_D9 },
        new() { BitsCount = 9, Code = 0x01_DA },
        new() { BitsCount = 9, Code = 0x01_DB },
        new() { BitsCount = 9, Code = 0x01_DC },
        new() { BitsCount = 9, Code = 0x01_DD },
        new() { BitsCount = 9, Code = 0x01_DE },
        new() { BitsCount = 9, Code = 0x01_DF },
        new() { BitsCount = 9, Code = 0x01_E0 },
        new() { BitsCount = 9, Code = 0x01_E1 },
        new() { BitsCount = 9, Code = 0x01_E2 },
        new() { BitsCount = 9, Code = 0x01_E3 },
        new() { BitsCount = 9, Code = 0x01_E4 },
        new() { BitsCount = 9, Code = 0x01_E5 },
        new() { BitsCount = 9, Code = 0x01_E6 },
        new() { BitsCount = 9, Code = 0x01_E7 },
        new() { BitsCount = 9, Code = 0x01_E8 },
        new() { BitsCount = 9, Code = 0x01_E9 },
        new() { BitsCount = 9, Code = 0x01_EA },
        new() { BitsCount = 9, Code = 0x01_EB },
        new() { BitsCount = 9, Code = 0x01_EC },
        new() { BitsCount = 9, Code = 0x01_ED },
        new() { BitsCount = 9, Code = 0x01_EE },
        new() { BitsCount = 9, Code = 0x01_EF },
        new() { BitsCount = 9, Code = 0x01_F0 },
        new() { BitsCount = 9, Code = 0x01_F1 },
        new() { BitsCount = 9, Code = 0x01_F2 },
        new() { BitsCount = 9, Code = 0x01_F3 },
        new() { BitsCount = 9, Code = 0x01_F4 },
        new() { BitsCount = 9, Code = 0x01_F5 },
        new() { BitsCount = 9, Code = 0x01_F6 },
        new() { BitsCount = 9, Code = 0x01_F7 },
        new() { BitsCount = 9, Code = 0x01_F8 },
        new() { BitsCount = 9, Code = 0x01_F9 },
        new() { BitsCount = 9, Code = 0x01_FA },
        new() { BitsCount = 9, Code = 0x01_FB },
        new() { BitsCount = 7, Code = 0x00_17 },
        new() { BitsCount = 6, Code = 0x00_00 },
        new() { BitsCount = 6, Code = 0x00_01 },
        new() { BitsCount = 6, Code = 0x00_02 },
        new() { BitsCount = 6, Code = 0x00_03 },
        new() { BitsCount = 7, Code = 0x00_08 },
        new() { BitsCount = 7, Code = 0x00_09 },
        new() { BitsCount = 7, Code = 0x00_0A },
        new() { BitsCount = 7, Code = 0x00_0B },
        new() { BitsCount = 7, Code = 0x00_0C },
        new() { BitsCount = 7, Code = 0x00_0D },
        new() { BitsCount = 7, Code = 0x00_0E },
        new() { BitsCount = 7, Code = 0x00_0F },
        new() { BitsCount = 7, Code = 0x00_10 },
        new() { BitsCount = 7, Code = 0x00_11 },
        new() { BitsCount = 7, Code = 0x00_12 },
        new() { BitsCount = 7, Code = 0x00_13 },
        new() { BitsCount = 9, Code = 0x01_FC },
        new() { BitsCount = 9, Code = 0x01_FD },
    ];

    public void CalculateStat(Span<int> groups)
    {
        Span<HuffTempStat> tempStats = stackalloc HuffTempStat[Common.HuffSymbolsCount];
        var total = MakeSortedTempStats(tempStats);

        NextStat = Common.HuffRecalcLength;
        var pos = 0;
        var totalCount = 0;
        for (var group = 0; group < 14; group++)
        {
            var avgGroup = (total - totalCount) / (16 - group);
            var iInner = 0;
            var n = 0;
            var nnInner = 0;
            for (var bitsCountInner = 0; ; bitsCountInner++)
            {
                var over = 0;
                var itemsCount = 1 << bitsCountInner;
                if (pos + iInner + itemsCount > Common.HuffSymbolsCount)
                {
                    itemsCount = Common.HuffSymbolsCount - pos;
                    over = 1;
                }

                for (; iInner < itemsCount; iInner++)
                {
                    nnInner += tempStats[pos + iInner].N;
                }

                if (over != 0 || bitsCountInner >= 8 || nnInner > avgGroup)
                {
                    if (bitsCountInner == 0 || float.Abs(n - avgGroup) < float.Abs(nnInner - avgGroup))
                    {
                        n = nnInner;
                    }
                    else
                    {
                        bitsCountInner--;
                    }

                    AddGroup(groups, group, bitsCountInner);
                    totalCount += n;
                    pos += 1 << bitsCountInner;
                    break;
                }
                else
                {
                    n = nnInner;
                }
            }
        }

        var bestBitsCount = 0;
        var bestBitsCount15 = 0;
        var best = 0x7F_FF_FF_FF;
        var i = 0;
        var nn = 0;
        var left = 0;

        int bitsCount;
        int bitsCount15;
        int j;
        for (j = pos; j < Common.HuffSymbolsCount; j++)
        {
            left += tempStats[j].N;
        }

        for (bitsCount = 0; ; bitsCount++)
        {
            var itemsCount = 1 << bitsCount;
            if (pos + i + itemsCount > Common.HuffSymbolsCount)
            {
                break;
            }

            for (; i < itemsCount; i++)
            {
                nn += tempStats[pos + i].N;
            }

            var itemsCount15 = Common.HuffSymbolsCount - (pos + i);
            for (bitsCount15 = 0; ; bitsCount15++)
            {
                if (1 << bitsCount15 >= itemsCount15)
                {
                    break;
                }
            }

            if (left < nn)
            {
                throw new InvalidOperationException($"Internal error in {nameof(EncoderStat)}.{nameof(CalculateStat)}");
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
                    break; // For performance
                }
            }
        }

        var pos15 = pos + (1 << bestBitsCount);
        AddGroup(groups, 14, bestBitsCount);
        AddGroup(groups, 15, bestBitsCount15);

        pos = 0;
        for (j = 0; j < 16; j++)
        {
            var bitsCountInner = groups[j];
            var itemsCount = 1 << bitsCountInner;
            var maxK = int.Min(itemsCount, Common.HuffSymbolsCount - pos);
            for (var k = 0; k < maxK; k++)
            {
                var symbol = tempStats[pos + k].I;
                SymbolTable[symbol].BitsCount = (short)(bitsCountInner + 4);
                SymbolTable[symbol].Code = (ushort)((j << bitsCountInner) | k);
            }

            pos += 1 << bitsCountInner;
        }
    }

    private static void AddGroup(Span<int> groups, int group, int bitsCount)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(bitsCount, 8);

        // Bubble sort
        int j;
        for (j = group; j > 0 && bitsCount < groups[j - 1]; j--)
        {
            groups[j] = groups[j - 1];
        }

        groups[j] = bitsCount;
    }

    [DebuggerDisplay("BitsCount={BitsCount}, Code=0x{Code:X4}")]
    public struct Symbol
    {
        public short BitsCount;
        public ushort Code;
    }
}
