// -----------------------------------------------------------------------
// <copyright file="SubsystemList.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using SageCore.FileSystem.Ini;
using SageCore.Utilities;

namespace SageCore.Subsystems;

/// <summary>
/// Manages a collection of subsystems, providing methods to initialize, reset, and shut down all subsystems.
/// </summary>
internal sealed class SubsystemList : IDisposable
{
    private static SubsystemList? _instance;

    private readonly List<SubsystemBase> _subsystems = [];

    private bool _disposed;

    private SubsystemList() { }

    /// <summary>
    /// Gets the singleton instance of the <see cref="SubsystemList"/>.
    /// </summary>
    public static SubsystemList Instance => _instance ??= new SubsystemList();

    /// <summary>
    /// Gets a read-only collection of the subsystems managed by this list.
    /// </summary>
    public ReadOnlyCollection<SubsystemBase> Subsystems => _subsystems.AsReadOnly();

    /// <summary>
    /// Initializes a subsystem with optional INI file loading.
    /// </summary>
    /// <param name="subsystem">The subsystem to initialize.</param>
    /// <param name="name">The name to assign to the subsystem.</param>
    /// <param name="path1">Optional path to the first INI file to load (overwrites existing data).</param>
    /// <param name="path2">Optional path to the second INI file to load (creates overrides).</param>
    /// <param name="dirPath">Optional directory path to load multiple INI files from (overwrites existing data).</param>
    /// <param name="xfer">Optional <see cref="Xfer"/> instance for custom data processing during INI loading.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the <see cref="SubsystemList"/> has been disposed.</exception>
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

        using IniFile init = new();
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
