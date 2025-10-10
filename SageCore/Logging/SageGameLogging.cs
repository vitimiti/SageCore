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

    [LoggerMessage(Level = LogLevel.Trace, Message = "Polled event: {EventType}")]
    public static partial void LogEventPolled(ILogger logger, string eventType);

    [LoggerMessage(Level = LogLevel.Information, Message = "Quit event received - stopping game loop")]
    public static partial void LogQuitEventReceived(ILogger logger);

    [LoggerMessage(Level = LogLevel.Trace, Message = "Update started")]
    public static partial void LogUpdateStarted(ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Update finished - Total time: {TotalTimeMs}ms, Delta: {DeltaTimeMs}ms"
    )]
    public static partial void LogUpdateFinished(ILogger logger, double totalTimeMs, double deltaTimeMs);

    [LoggerMessage(Level = LogLevel.Trace, Message = "Draw started")]
    public static partial void LogDrawStarted(ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Draw finished - Total time: {TotalTimeMs}ms, Delta: {DeltaTimeMs}ms"
    )]
    public static partial void LogDrawFinished(ILogger logger, double totalTimeMs, double deltaTimeMs);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Platform system disposed")]
    public static partial void LogPlatformSystemDisposed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Screen disposed")]
    public static partial void LogScreenDisposed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "SDL quit called")]
    public static partial void LogSdlQuit(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Attempted to run game that has already been disposed")]
    public static partial void LogRunOnDisposedGame(ILogger logger);

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

    [LoggerMessage(Level = LogLevel.Information, Message = "Application Data Directory: {ApplicationDataDirectory}")]
    public static partial void LogApplicationDataDirectory(ILogger logger, string applicationDataDirectory);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Local Application Data Directory: {LocalApplicationDataDirectory}"
    )]
    public static partial void LogLocalApplicationDataDirectory(ILogger logger, string localApplicationDataDirectory);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Common Application Data Directory: {CommonApplicationDataDirectory}"
    )]
    public static partial void LogCommonApplicationDataDirectory(ILogger logger, string commonApplicationDataDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Desktop Directory: {DesktopDirectory}")]
    public static partial void LogDesktopDirectory(ILogger logger, string desktopDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Documents Directory: {DocumentsDirectory}")]
    public static partial void LogDocumentsDirectory(ILogger logger, string documentsDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "User Profile Directory: {UserProfileDirectory}")]
    public static partial void LogUserProfileDirectory(ILogger logger, string userProfileDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Program Files Directory: {ProgramFilesDirectory}")]
    public static partial void LogProgramFilesDirectory(ILogger logger, string programFilesDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Program Files (x86) Directory: {ProgramFilesX86Directory}")]
    public static partial void LogProgramFilesX86Directory(ILogger logger, string programFilesX86Directory);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Common Program Files Directory: {CommonProgramFilesDirectory}"
    )]
    public static partial void LogCommonProgramFilesDirectory(ILogger logger, string commonProgramFilesDirectory);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Common Program Files (x86) Directory: {CommonProgramFilesX86Directory}"
    )]
    public static partial void LogCommonProgramFilesX86Directory(ILogger logger, string commonProgramFilesX86Directory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Temporary Directory: {TemporaryDirectory}")]
    public static partial void LogTemporaryDirectory(ILogger logger, string temporaryDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "My Documents Directory: {MyDocumentsDirectory}")]
    public static partial void LogMyDocumentsDirectory(ILogger logger, string myDocumentsDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "My Music Directory: {MyMusicDirectory}")]
    public static partial void LogMyMusicDirectory(ILogger logger, string myMusicDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "My Pictures Directory: {MyPicturesDirectory}")]
    public static partial void LogMyPicturesDirectory(ILogger logger, string myPicturesDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "My Videos Directory: {MyVideosDirectory}")]
    public static partial void LogMyVideosDirectory(ILogger logger, string myVideosDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Startup Directory: {StartupDirectory}")]
    public static partial void LogStartupDirectory(ILogger logger, string startupDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Start Menu Directory: {StartMenuDirectory}")]
    public static partial void LogStartMenuDirectory(ILogger logger, string startMenuDirectory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Common Start Menu Directory: {CommonStartMenuDirectory}")]
    public static partial void LogCommonStartMenuDirectory(ILogger logger, string commonStartMenuDirectory);

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
}
