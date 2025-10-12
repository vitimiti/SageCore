// -----------------------------------------------------------------------
// <copyright file="Ini.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.FileSystem.Utilities;

namespace SageCore.FileSystem.Ini;

public class Ini
{
    public void LoadDirectory(string dirName, bool subDirs, IniLoadType loadType, Xfer? xfer = null) =>
        throw new NotImplementedException();

    public void Load(string name, IniLoadType loadType, Xfer? xfer = null) => throw new NotImplementedException();
}
