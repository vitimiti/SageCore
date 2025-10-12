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
public abstract class SubsystemBase : IDisposable
{
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubsystemBase"/> class.
    /// </summary>
    protected SubsystemBase() { }

    /// <summary>
    /// Gets or sets the name of the subsystem.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Initializes the subsystem.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Called after loading is complete to perform any necessary post-processing.
    /// </summary>
    public virtual void PostProcessLoad() { }

    /// <summary>
    /// Resets the subsystem to its initial state.
    /// </summary>
    public abstract void Reset();

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Updates the subsystem state.
    /// </summary>
    protected abstract void Update();

    /// <summary>
    /// Renders the subsystem's visual elements.
    /// </summary>
    protected abstract void Draw();

    /// <summary>
    /// Disposes of the resources used by the subsystem.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from Dispose (true) or from a finalizer (false).</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        _disposedValue = true;
    }
}
