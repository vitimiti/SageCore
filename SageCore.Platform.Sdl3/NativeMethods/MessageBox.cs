// -----------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SageCore.Platform.Sdl3.NativeMethods;

internal static partial class Sdl
{
    public const uint MessageBoxError = 0x10;
    public const uint MessageBoxButtonsLeftToRight = 0x80;

    private const string DllName = "SDL3";

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_ShowSimpleMessageBox", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool ShowSimpleMessageBox(uint flags, string title, string message, nint window);
}
