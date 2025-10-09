// -----------------------------------------------------------------------
// <copyright file="Video.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SageCore.NativeMethods.Sdl3;

internal static partial class Sdl
{
    public const ulong WindowFullscreen = 0x00_01;
    public const ulong WindowResizable = 0x00_20;
    public const ulong WindowMouseCapture = 0x40_00;

    [NativeMarshalling(typeof(SafeHandleMarshaller<WindowSafeHandle>))]
    public sealed partial class WindowSafeHandle : SafeHandle
    {
        public WindowSafeHandle()
            : base(invalidHandleValue: nint.Zero, ownsHandle: true) { }

        public override bool IsInvalid => handle == nint.Zero;

        public static WindowSafeHandle Create(string title, int width, int height, ulong flags) =>
            CreateWindow(title, width, height, flags);

        protected override bool ReleaseHandle()
        {
            DestroyWindow(handle);
            return true;
        }

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_CreateWindow", StringMarshalling = StringMarshalling.Utf8)]
        private static partial WindowSafeHandle CreateWindow(string title, int w, int h, ulong flags);

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_DestroyWindow")]
        private static partial void DestroyWindow(nint window);
    }
}
