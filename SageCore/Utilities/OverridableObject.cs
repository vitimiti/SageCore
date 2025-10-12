// -----------------------------------------------------------------------
// <copyright file="OverridableObject.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Abstractions;
using SageCore.Attributes;

namespace SageCore.Utilities;

/// <summary>
/// Represents an object that can be overridden and supports object pooling.
/// </summary>
/// <remarks>
/// This class allows for creating a chain of overrides, where each override can point to the next one.
/// It also implements the <see cref="IPooledResettable"/> interface to support resetting its state when returned to an object pool.
/// </remarks>
[Pooled]
public class OverridableObject : IPooledResettable
{
    private bool _isOverride;

    /// <summary>
    /// Gets or sets the next override in the chain.
    /// </summary>
    public OverridableObject? NextOverride { get; set; }

    /// <summary>
    /// Gets the final override in the chain.
    /// </summary>
    public OverridableObject FinalOverride => NextOverride is not null ? NextOverride.FinalOverride : this;

    /// <summary>
    /// Marks this object as an override.
    /// </summary>
    public void MarkAsOverride() => _isOverride = true;

    /// <summary>
    /// Deletes all overrides in the chain, returning the base object.
    /// </summary>
    /// <returns>The base object with all overrides removed or <see langword="null"/>.</returns>
    /// <remarks>
    /// If this object is an override, it resets itself and returns <see langword="null"/>.
    /// If it is not an override, it recursively calls <see cref="DeleteOverrides"/> on the next override in the chain.
    /// </remarks>
    public OverridableObject? DeleteOverrides()
    {
        if (_isOverride)
        {
            Reset();
            return null;
        }

        if (NextOverride is not null)
        {
            NextOverride = NextOverride.DeleteOverrides();
        }

        return this;
    }

    /// <summary>
    /// Resets the object's state, clearing the override flag and the next override reference.
    /// </summary>
    public void Reset()
    {
        _isOverride = false;
        NextOverride = null;
    }
}
