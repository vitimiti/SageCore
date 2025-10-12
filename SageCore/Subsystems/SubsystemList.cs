// -----------------------------------------------------------------------
// <copyright file="SubsystemList.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using SageCore.Utils;

namespace SageCore.Subsystems;

internal sealed class SubsystemList : IDisposable
{
    private readonly List<SubsystemBase> _subsystems = [];

    private bool _disposed;

    public ReadOnlyCollection<SubsystemBase> Subsystems => _subsystems.AsReadOnly();

    public void InitializeSubsystem(
        SubsystemBase subsystem,
        string name,
        string? path1 = null,
        string? path2 = null,
        string? dirPath = null,
        Xfer? xfer = null
    )
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        subsystem.Name = name;
        subsystem.Initialize();

        // TODO: INI initialization using path1, path2, dirPath, xfer
        _subsystems.Add(subsystem);
    }

    public void PostProcessLoadAll()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        foreach (SubsystemBase subsystem in _subsystems)
        {
            subsystem.PostProcessLoad();
        }
    }

    public void ResetAll()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        foreach (SubsystemBase subsystem in _subsystems)
        {
            subsystem.Reset();
        }
    }

    public void ShutdownAll()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        foreach (SubsystemBase subsystem in _subsystems)
        {
            subsystem.Dispose();
        }

        _subsystems.Clear();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        ShutdownAll();
        _disposed = true;
    }
}
