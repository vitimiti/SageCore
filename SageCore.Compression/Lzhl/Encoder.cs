// -----------------------------------------------------------------------
// <copyright file="Encoder.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
using DispItem = (int BitsCount, ushort Bits);
using MatchOverItem = (int Symbol, int BitsCount, ushort Bits);
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly

namespace SageCore.Compression.Lzhl;

[DebuggerDisplay("Bits={_bits}, BitsCount={_bitsCount}, DestinationIndex={_destinationIndex}, NextStat={_nextStat}")]
internal sealed class Encoder([NotNull] EncoderStat stat, [NotNull] byte[] destination)
{
    public const int MaxMatchOver = 517;
    public const int MaxRaw = 64;

    private static readonly MatchOverItem[] MatchOverTable =
    [
        (264, 1, 0x00),
        (265, 2, 0x00),
        (265, 2, 0x02),
        (266, 3, 0x00),
        (266, 3, 0x02),
        (266, 3, 0x04),
        (266, 3, 0x06),
        (267, 4, 0x00),
        (267, 4, 0x02),
        (267, 4, 0x04),
        (267, 4, 0x06),
        (267, 4, 0x08),
        (267, 4, 0x0A),
        (267, 4, 0x0C),
        (267, 4, 0x0E),
    ];

    private static readonly DispItem[] DispTable =
    [
        (3, 0x00_00),
        (3, 0x00_01),
        (4, 0x00_04),
        (4, 0x00_05),
        (5, 0x00_0C),
        (5, 0x00_0D),
        (5, 0x00_0E),
        (5, 0x00_0F),
        (6, 0x00_20),
        (6, 0x00_21),
        (6, 0x00_22),
        (6, 0x00_23),
        (6, 0x00_24),
        (6, 0x00_25),
        (6, 0x00_26),
        (6, 0x00_27),
        (7, 0x00_50),
        (7, 0x00_51),
        (7, 0x00_52),
        (7, 0x00_53),
        (7, 0x00_54),
        (7, 0x00_55),
        (7, 0x00_56),
        (7, 0x00_57),
        (7, 0x00_58),
        (7, 0x00_59),
        (7, 0x00_5A),
        (7, 0x00_5B),
        (7, 0x00_5C),
        (7, 0x00_5D),
        (7, 0x00_5E),
        (7, 0x00_5F),
        (8, 0x00_C0),
        (8, 0x00_C1),
        (8, 0x00_C2),
        (8, 0x00_C3),
        (8, 0x00_C4),
        (8, 0x00_C5),
        (8, 0x00_C6),
        (8, 0x00_C7),
        (8, 0x00_C8),
        (8, 0x00_C9),
        (8, 0x00_CA),
        (8, 0x00_CB),
        (8, 0x00_CC),
        (8, 0x00_CD),
        (8, 0x00_CE),
        (8, 0x00_CF),
        (8, 0x00_D0),
        (8, 0x00_D1),
        (8, 0x00_D2),
        (8, 0x00_D3),
        (8, 0x00_D4),
        (8, 0x00_D5),
        (8, 0x00_D6),
        (8, 0x00_D7),
        (8, 0x00_D8),
        (8, 0x00_D9),
        (8, 0x00_DA),
        (8, 0x00_DB),
        (8, 0x00_DC),
        (8, 0x00_DD),
        (8, 0x00_DE),
        (8, 0x00_DF),
        (9, 0x01_C0),
        (9, 0x01_C1),
        (9, 0x01_C2),
        (9, 0x01_C3),
        (9, 0x01_C4),
        (9, 0x01_C5),
        (9, 0x01_C6),
        (9, 0x01_C7),
        (9, 0x01_C8),
        (9, 0x01_C9),
        (9, 0x01_CA),
        (9, 0x01_CB),
        (9, 0x01_CC),
        (9, 0x01_CD),
        (9, 0x01_CE),
        (9, 0x01_CF),
        (9, 0x01_D0),
        (9, 0x01_D1),
        (9, 0x01_D2),
        (9, 0x01_D3),
        (9, 0x01_D4),
        (9, 0x01_D5),
        (9, 0x01_D6),
        (9, 0x01_D7),
        (9, 0x01_D8),
        (9, 0x01_D9),
        (9, 0x01_DA),
        (9, 0x01_DB),
        (9, 0x01_DC),
        (9, 0x01_DD),
        (9, 0x01_DE),
        (9, 0x01_DF),
        (9, 0x01_E0),
        (9, 0x01_E1),
        (9, 0x01_E2),
        (9, 0x01_E3),
        (9, 0x01_E4),
        (9, 0x01_E5),
        (9, 0x01_E6),
        (9, 0x01_E7),
        (9, 0x01_E8),
        (9, 0x01_E9),
        (9, 0x01_EA),
        (9, 0x01_EB),
        (9, 0x01_EC),
        (9, 0x01_ED),
        (9, 0x01_EE),
        (9, 0x01_EF),
        (9, 0x01_F0),
        (9, 0x01_F1),
        (9, 0x01_F2),
        (9, 0x01_F3),
        (9, 0x01_F4),
        (9, 0x01_F5),
        (9, 0x01_F6),
        (9, 0x01_F7),
        (9, 0x01_F8),
        (9, 0x01_F9),
        (9, 0x01_FA),
        (9, 0x01_FB),
        (9, 0x01_FC),
        (9, 0x01_FD),
        (9, 0x01_FE),
        (9, 0x01_FF),
    ];

    private readonly EncoderStat _stat = stat;
    private readonly short[] _sstat = stat.Stat;
    private int _nextStat = stat.NextStat;
    private int _destinationIndex;
    private int _bits;
    private int _bitsCount;

    public static int CalculateMaximumBufferSize(int rawSize) => rawSize + (rawSize >> 1) + 32;

    public void PutRaw(ReadOnlySpan<byte> source)
    {
        foreach (var b in source)
        {
            Put(b);
        }
    }

    public void PutMatch(Span<byte> source, int matchOver, int disp)
    {
        if (source.Length > MaxRaw)
        {
            throw new ArgumentOutOfRangeException(
                nameof(source),
                $"Source length must be less than or equal to {MaxRaw}."
            );
        }

        ArgumentOutOfRangeException.ThrowIfGreaterThan(matchOver, MaxMatchOver);
        ArgumentOutOfRangeException.ThrowIfNegative(disp);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(disp, Constants.BufferSize);

        PutRaw(source);

        if (matchOver < 8)
        {
            Put((ushort)(256 + matchOver));
        }
        else if (matchOver < 38)
        {
            matchOver -= 8;
            MatchOverItem matchOverItem = MatchOverTable[matchOver >> 1]; // matchOver / 2
            Put((ushort)matchOverItem.Symbol, matchOverItem.BitsCount, (uint)(matchOverItem.Bits | (matchOver & 0x01))); // matchOver % 2
        }
        else
        {
            matchOver -= 38;
            MatchOverItem item = MatchOverTable[matchOver >> 5]; // matchOver / 32
            Put((ushort)(item.Symbol + 4));
            PutBits(item.BitsCount + 4, (uint)((item.Bits << 4) | (matchOver & 0x1F))); // matchOver % 32
        }

        DispItem dispItem = DispTable[disp >> (Constants.BufferBits - 7)];
        var bitsCount = dispItem.BitsCount + (Constants.BufferBits - 7);
        var bits =
            ((uint)dispItem.Bits << (Constants.BufferBits - 7))
            | ((uint)(disp & ((1 << (Constants.BufferBits - 7)) - 1)));

        if (bitsCount > 16)
        {
            throw new InvalidOperationException("Invalid disp bits count.");
        }

        PutBits(bitsCount, bits);
    }

    public long Flush()
    {
        Put(Constants.HuffSymbolsCount - 1);
        while (_bitsCount > 0)
        {
            destination[_destinationIndex++] = (byte)(_bits >> 24);
            _bitsCount -= 8;
            _bits <<= 8;
        }

        return _destinationIndex;
    }

    private void CalculateStat()
    {
        _nextStat = 2; // To avoid recursion, >= 2
        Put(Constants.HuffSymbolsCount - 2);

        Span<int> groups = stackalloc int[16];
        _stat.CalculateStat(groups);

        var lastBitsCount = 0;
        for (var i = 0; i < 16; ++i)
        {
            var bitsCount = groups[i];
            if (bitsCount < lastBitsCount || bitsCount > 8)
            {
                throw new InvalidOperationException("Invalid group bits count.");
            }

            var delta = bitsCount - lastBitsCount;
            lastBitsCount = bitsCount;
            PutBits(delta + 1, 1);
        }
    }

    private void PutBits(int codeBits, uint code)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(codeBits, 16);

        _bits |= (int)(code << (32 - _bitsCount - codeBits));
        _bitsCount += codeBits;
        if (_bitsCount >= 16)
        {
            destination[_destinationIndex++] = (byte)(_bits >> 24);
            destination[_destinationIndex++] = (byte)(_bits >> 16);
            _bitsCount -= 16;
            _bits <<= 16;
        }
    }

    private void Put(ushort symbol)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(symbol, Constants.HuffSymbolsCount);

        if (--_nextStat <= 0)
        {
            CalculateStat();
        }

        ++_sstat[symbol];
        EncoderStat.Symbol item = _stat.SymbolTable[symbol];
        if (item.BitsCount < 0)
        {
            throw new InvalidOperationException("Symbol bits not calculated.");
        }

        PutBits(item.BitsCount, item.Code);
    }

    private void Put(ushort symbol, int codeBits, uint code)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(symbol, Constants.HuffSymbolsCount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(codeBits, 4);

        if (--_nextStat <= 0)
        {
            CalculateStat();
        }

        ++_sstat[symbol];
        EncoderStat.Symbol item = _stat.SymbolTable[symbol];
        if (item.BitsCount < 0)
        {
            throw new InvalidOperationException("Symbol bits not calculated.");
        }

        var bitsCount = item.BitsCount;
        PutBits(bitsCount + codeBits, ((uint)item.Code << (ushort)codeBits) | code);
    }
}
