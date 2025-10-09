// -----------------------------------------------------------------------
// <copyright file="Log.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SageCore.NativeMethods.Sdl3;

internal static unsafe partial class Sdl
{
    public delegate void LogOutputFunction(LogCategory category, LogPriority priority, string message);

#pragma warning disable SA1023 // Dereference and access of symbols should be spaced correctly
    private static readonly delegate* unmanaged[Cdecl]<nint, int, LogPriority, byte*, void> LogOutputFunctionPtr =
#pragma warning restore SA1023 // Dereference and access of symbols should be spaced correctly
        &LogOutputFunctionImpl;

    public static GCHandle LogOutputFunctionHandle { get; set; }

    public static void SetLogOutputFunction(LogOutputFunction callback)
    {
        LogOutputFunctionHandle = GCHandle.Alloc(callback);
        SetLogOutputFunctionUnsafe(LogOutputFunctionPtr, GCHandle.ToIntPtr(LogOutputFunctionHandle));
    }

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_SetLogPriorities")]
    public static partial void SetLogPriorities(LogPriority priority);

    [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
    [LibraryImport(DllName, EntryPoint = "SDL_SetLogOutputFunction")]
    private static partial void SetLogOutputFunctionUnsafe(
#pragma warning disable SA1023 // Dereference and access of symbols should be spaced correctly
#pragma warning disable SA1114 // Parameter list should follow declaration
        delegate* unmanaged[Cdecl]<nint, int, LogPriority, byte*, void> callback,
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore SA1023 // Dereference and access of symbols should be spaced correctly
        nint userdata
    );

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void LogOutputFunctionImpl(nint userdata, int category, LogPriority priority, byte* message)
    {
        var callback = GCHandle.FromIntPtr(userdata).Target as LogOutputFunction;
        callback?.Invoke(
            (LogCategory)category,
            priority,
            Utf8StringMarshaller.ConvertToManaged(message) ?? string.Empty
        );
    }

    public enum LogCategory
    {
        Application,
        Error,
        Assert,
        System,
        Audio,
        Video,
        Render,
        Input,
        Test,
        Gpu,
    }

    public enum LogPriority
    {
        Invalid,
        Trace,
        Verbose,
        Debug,
        Info,
        Warn,
        Error,
        Critical,
    }
}
