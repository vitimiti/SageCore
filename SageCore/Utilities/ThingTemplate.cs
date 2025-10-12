// -----------------------------------------------------------------------
// <copyright file="ThingTemplate.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Abstractions;
using SageCore.Attributes;

namespace SageCore.Utilities;

[Pooled]
public sealed class ThingTemplate : OverridableObject, IPooledResettable { }
