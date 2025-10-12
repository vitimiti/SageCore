// -----------------------------------------------------------------------
// <copyright file="IPooledResettable.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Abstractions.Utilities;

/// <summary>
/// Defines a contract for objects that can be reset when returned to an object pool.
/// </summary>
public interface IPooledResettable
{
    /// <summary>
    /// Resets the object's state to its initial configuration.
    /// </summary>
    void Reset();
}
