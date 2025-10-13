// -----------------------------------------------------------------------
// <copyright file="Bucket.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Abstractions.Utilities;
using SageCore.Attributes;
using SageCore.Enums;

namespace SageCore.Utilities;

[Pooled]
internal sealed class Bucket : IPooledResettable
{
    public Bucket? NextInSocket { get; set; }

    public NameKeyType Key { get; set; } = NameKeyType.Invalid;

    public string? Name { get; set; }

    public void Reset()
    {
        NextInSocket = null;
        Key = NameKeyType.Invalid;
        Name = null;
    }
}
