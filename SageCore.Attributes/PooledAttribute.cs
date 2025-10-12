// -----------------------------------------------------------------------
// <copyright file="PooledAttribute.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Attributes;

/// <summary>
/// Indicates that a class is intended to be used with object pooling.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PooledAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a value indicating whether the object's <c>Reset</c> method should be called automatically when the object is returned to the pool.
    /// </summary>
    public bool CallResetOnReturn { get; set; } = true;
}
