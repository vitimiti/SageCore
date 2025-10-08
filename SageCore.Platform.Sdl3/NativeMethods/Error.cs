// -----------------------------------------------------------------------
// <copyright file="Error.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using SageCore.Platform.Sdl3.NativeMethods.CustomMarshallers;

namespace SageCore.Platform.Sdl3.NativeMethods;

internal static unsafe partial class Sdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(
        DllName,
        EntryPoint = "SDL_GetError",
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(SdlOwnedUtf8StringMarshaller)
    )]
    public static partial string? GetError();
}
