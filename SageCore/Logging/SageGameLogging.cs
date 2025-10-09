// -----------------------------------------------------------------------
// <copyright file="SageGameLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class SageGameLogging
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Starting game loop")]
    public static partial void LogGameLoopStarting(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Game loop ended")]
    public static partial void LogGameLoopEnded(ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Game loop iteration - Total time: {TotalTimeMs}ms, Delta: {DeltaTimeMs}ms"
    )]
    public static partial void LogGameLoopIteration(ILogger logger, double totalTimeMs, double deltaTimeMs);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Platform system disposed")]
    public static partial void LogPlatformSystemDisposed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Screen disposed")]
    public static partial void LogScreenDisposed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "SDL quit called")]
    public static partial void LogSdlQuit(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Attempted to run game that has already been disposed")]
    public static partial void LogRunOnDisposedGame(ILogger logger);

    // System Information Logging
    [LoggerMessage(Level = LogLevel.Information, Message = "=== SYSTEM INFORMATION ===")]
    public static partial void LogSystemInfoHeader(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Operating System: {OSDescription}")]
    public static partial void LogOperatingSystem(ILogger logger, string osDescription);

    [LoggerMessage(Level = LogLevel.Information, Message = "OS Version: {OSVersion}")]
    public static partial void LogOSVersion(ILogger logger, string osVersion);

    [LoggerMessage(Level = LogLevel.Information, Message = "Architecture: {Architecture}")]
    public static partial void LogArchitecture(ILogger logger, string architecture);

    [LoggerMessage(Level = LogLevel.Information, Message = "Process Architecture: {ProcessArchitecture}")]
    public static partial void LogProcessArchitecture(ILogger logger, string processArchitecture);

    [LoggerMessage(Level = LogLevel.Information, Message = "Framework: {FrameworkDescription}")]
    public static partial void LogFramework(ILogger logger, string frameworkDescription);

    [LoggerMessage(Level = LogLevel.Information, Message = "Runtime Version: {RuntimeVersion}")]
    public static partial void LogRuntimeVersion(ILogger logger, string runtimeVersion);

    [LoggerMessage(Level = LogLevel.Information, Message = "Processor Count: {ProcessorCount}")]
    public static partial void LogProcessorCount(ILogger logger, int processorCount);

    [LoggerMessage(Level = LogLevel.Information, Message = "Machine Name: {MachineName}")]
    public static partial void LogMachineName(ILogger logger, string machineName);

    [LoggerMessage(Level = LogLevel.Information, Message = "User Name: {UserName}")]
    public static partial void LogUserName(ILogger logger, string userName);

    [LoggerMessage(Level = LogLevel.Information, Message = "User Domain: {UserDomainName}")]
    public static partial void LogUserDomain(ILogger logger, string userDomainName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Working Directory: {WorkingDirectory}")]
    public static partial void LogWorkingDirectory(ILogger logger, string workingDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "System Directory: {SystemDirectory}")]
    public static partial void LogSystemDirectory(ILogger logger, string systemDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Current Process: {ProcessName} (PID: {ProcessId})")]
    public static partial void LogCurrentProcess(ILogger logger, string processName, int processId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Process Started: {StartTime}")]
    public static partial void LogProcessStartTime(ILogger logger, DateTime startTime);

    [LoggerMessage(Level = LogLevel.Information, Message = "Total Physical Memory: {TotalMemoryMB} MB")]
    public static partial void LogTotalPhysicalMemory(ILogger logger, long totalMemoryMB);

    [LoggerMessage(Level = LogLevel.Information, Message = "Available Physical Memory: {AvailableMemoryMB} MB")]
    public static partial void LogAvailablePhysicalMemory(ILogger logger, long availableMemoryMb);

    [LoggerMessage(Level = LogLevel.Information, Message = "GC Total Memory: {GCMemoryMB} MB")]
    public static partial void LogGCMemory(ILogger logger, long gcMemoryMb);

    [LoggerMessage(Level = LogLevel.Information, Message = "Is 64-bit Process: {Is64Bit}")]
    public static partial void LogIs64BitProcess(ILogger logger, bool is64Bit);

    [LoggerMessage(Level = LogLevel.Information, Message = "Is 64-bit OS: {Is64BitOS}")]
    public static partial void LogIs64BitOS(ILogger logger, bool is64BitOs);

    [LoggerMessage(Level = LogLevel.Information, Message = "CLR Version: {CLRVersion}")]
    public static partial void LogCLRVersion(ILogger logger, string clrVersion);

    [LoggerMessage(Level = LogLevel.Information, Message = "=== END SYSTEM INFORMATION ===")]
    public static partial void LogSystemInfoFooter(ILogger logger);
}
