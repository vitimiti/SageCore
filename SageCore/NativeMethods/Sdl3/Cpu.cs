// -----------------------------------------------------------------------
// <copyright file="Cpu.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SageCore.NativeMethods.Sdl3;

internal static partial class Sdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_GetSystemRAM")]
    public static partial int GetSystemRam();
}
