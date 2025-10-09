// -----------------------------------------------------------------------
// <copyright file="Events.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SageCore.NativeMethods.Sdl3;

internal static unsafe partial class Sdl
{
    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_PollEvent")]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool PollEvent(out Event sdlEvent);

    public enum EventType
    {
        Quit = 0x01_00,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct QuitEvent
    {
        public EventType Type;
        public uint Reserved;
        public ulong Timestamp;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Event
    {
        [FieldOffset(0)]
        public EventType Type;

        [FieldOffset(0)]
        public QuitEvent Quit;

        [FieldOffset(0)]
        private fixed byte _padding[128];
    }
}
