// -----------------------------------------------------------------------
// <copyright file="Image.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Abstractions.Utilities;
using SageCore.Attributes;
using SageCore.Models;

namespace SageCore.Utilities;

[Pooled]
public sealed partial class Image : IPooledResettable
{
    private uint _status;

    public string Name { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public Region2D UvCoords { get; set; } = new Region2D(Lo: new Coord2D(0, 0), Hi: new Coord2D(1, 1));

    public Coord2D Size { get; set; } = new Coord2D(0, 0);

    public void Reset() => throw new NotImplementedException();

    public IEnumerable<byte>? RawTextureData { get; set; }

    public uint Status => _status;

    public uint SetStatus(uint bit)
    {
        var previousStatus = _status;
        _status = BitSet(_status, bit);
        return previousStatus;
    }

    public uint ClearStatus(uint bit)
    {
        var previousStatus = _status;
        _status = BitClear(_status, bit);
        return previousStatus;
    }

    private static uint BitSet(uint x, uint i) => x | i;

    private static uint BitClear(uint x, uint i) => x & ~i;
}
