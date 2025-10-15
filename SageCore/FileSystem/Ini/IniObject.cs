// -----------------------------------------------------------------------
// <copyright file="IniObject.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Subsystems;

namespace SageCore.FileSystem.Ini;

internal static class IniObject
{
    public static void ParseObjectDefinition(IniFile ini)
    {
        var name = ini.GetNextToken();
    }
}
