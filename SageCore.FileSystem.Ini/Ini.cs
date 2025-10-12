// -----------------------------------------------------------------------
// <copyright file="Ini.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using SageCore.FileSystem.Utilities;

namespace SageCore.FileSystem.Ini;

public class Ini
{
    public void LoadDirectory([NotNull] string dirName, bool subDirs, IniLoadType loadType, Xfer? xfer = null)
    {
        if (string.IsNullOrWhiteSpace(dirName))
        {
            throw new ArgumentException("Directory name cannot be null or whitespace.", nameof(dirName));
        }

        if (string.IsNullOrEmpty(dirName))
        {
            throw new ArgumentException("Directory name cannot be null or empty.", nameof(dirName));
        }
    }

    public void Load(string name, IniLoadType loadType, Xfer? xfer = null) => throw new NotImplementedException();
}
