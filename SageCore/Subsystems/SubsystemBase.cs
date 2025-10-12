// -----------------------------------------------------------------------
// <copyright file="SubsystemBase.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Subsystems;

/// <summary>
/// The base class for all subsystems in the game engine.
/// </summary>
internal abstract class SubsystemBase : IDisposable
{
    private bool _disposedValue;

    protected SubsystemBase() { }

    /// <summary>
    /// Gets or sets the name of the subsystem.
    /// </summary>
    public string? Name { get; set; }

    public abstract void Initialize();

    public virtual void PostProcessLoad() { }

    public abstract void Reset();

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Update();

    protected abstract void Draw();

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        _disposedValue = true;
    }
}
