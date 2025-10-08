// -----------------------------------------------------------------------
// <copyright file="StdInc.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SageCore.Platform.Sdl3.NativeMethods;

internal static unsafe partial class Sdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_strdup")]
    public static partial byte* StrDup(byte* str);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_free")]
    public static partial void Free(void* ptr);
}
