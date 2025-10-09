// -----------------------------------------------------------------------
// <copyright file="Init.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SageCore.NativeMethods.Sdl3;

internal static partial class Sdl
{
    public const uint InitVideo = 0x20;

    public const string PropAppMetadataNameString = "SDL.app.metadata.name";
    public const string PropAppMetadataVersionString = "SDL.app.metadata.version";
    public const string PropAppMetadataIdentifierString = "SDL.app.metadata.identifier";
    public const string PropAppMetadataCreatorString = "SDL.app.metadata.creator";
    public const string PropAppMetadataCopyrightString = "SDL.app.metadata.copyright";
    public const string PropAppMetadataUrlString = "SDL.app.metadata.url";
    public const string PropAppMetadataTypeString = "SDL.app.metadata.type";

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_Init")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Init(uint flags);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_QuitSubSystem")]
    public static partial void QuitSubSystem(uint flags);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_Quit")]
    public static partial void Quit();

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_SetAppMetadataProperty", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool TrySetAppMetadataProperty(string name, string? value);
}
