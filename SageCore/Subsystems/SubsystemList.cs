// -----------------------------------------------------------------------
// <copyright file="SubsystemList.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using SageCore.FileSystem.Ini;
using SageCore.FileSystem.Utilities;

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

        Ini init = new();

        if (path1 is not null)
        {
            init.Load(path1, IniLoadType.Overwrite, xfer);
        }

        if (path2 is not null)
        {
            init.Load(path2, IniLoadType.CreateOverrides, xfer);
        }

        if (dirPath is not null)
        {
            init.LoadDirectory(dirPath, subDirs: true, IniLoadType.Overwrite, xfer);
        }

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
