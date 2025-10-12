// -----------------------------------------------------------------------
// <copyright file="SubsystemBase.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Subsystems;

internal abstract class SubsystemBase : IDisposable
{
    private bool _disposedValue;

    protected SubsystemBase() { }

    public string? Name { get; set; }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected abstract void Initialize();

    protected virtual void PostProcessLoad() { }

    protected abstract void Reset();

    protected abstract void Update();

    protected virtual void Draw() => throw new NotSupportedException("Draw not supported for this subsystem.");

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        _disposedValue = true;
    }
}
