// -----------------------------------------------------------------------
// <copyright file="IniObject.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.FileSystem.Ini;

internal static class IniObject
{
    public static void ParseObjectDefinition(Ini ini)
    {
        var name = ini.GetNextToken();
        throw new NotImplementedException();
    }
}
