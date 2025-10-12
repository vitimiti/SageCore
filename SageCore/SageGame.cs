// -----------------------------------------------------------------------
// <copyright file="SageGame.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using SageCore.Logging;
using SageCore.NativeMethods.Sdl3;
using SageCore.Systems;
using SageCore.Systems.VideoSystem;
using SageCore.Utilities;

namespace SageCore;

/// <summary>
/// A basic game class to represent a Sage game instance.
/// </summary>
[DebuggerDisplay("IsRunning = {IsRunning}")]
public sealed class SageGame : IDisposable
{
    private readonly ILogger _logger;
    private readonly SageGameOptions _options = new();
    private readonly GameTime _gameTime;
    private readonly PlatformSystem _platformSystem;
    private readonly Screen _screen;

    private bool _disposed;
    private bool _isRunning;

    /// <summary>
    /// Initializes a new instance of the <see cref="SageGame"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging game events.</param>
    /// <param name="options">The game options for configuring the game instance. If null, default options will be used.</param>
    public SageGame([NotNull] ILogger logger, Action<SageGameOptions>? options = null)
    {
        _logger = logger;
        options?.Invoke(_options);

        LogSystemInformation();

        CommonLogging.LogInitializing(_logger, nameof(SageGame));

        _gameTime = new GameTime(_logger);
        _platformSystem = new PlatformSystem(_logger, _options.AppOptions);
        _screen = new Screen(_logger, _options.ScreenOptions);

        CommonLogging.LogInitialized(_logger, nameof(SageGame));
    }

    /// <summary>
    /// Runs the game instance.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the game instance has been disposed.</exception>
    public void Run()
    {
        if (_disposed)
        {
            SageGameLogging.LogRunOnDisposedGame(_logger);
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        SageGameLogging.LogGameLoopStarting(_logger);
        _isRunning = true;

        while (_isRunning)
        {
            Update();
            Draw();

            // Log each iteration at debug level (can be useful for debugging but not too verbose)
            SageGameLogging.LogGameLoopIteration(
                _logger,
                _gameTime.TotalTime.TotalMilliseconds,
                _gameTime.DeltaTime.TotalMilliseconds
            );
        }

        SageGameLogging.LogGameLoopEnded(_logger);
    }

    /// <summary>
    /// Disposes of the game instance.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            CommonLogging.LogDisposing(_logger, nameof(SageGame));

            _platformSystem.Dispose();
            SageGameLogging.LogPlatformSystemDisposed(_logger);

            _screen.Dispose();
            SageGameLogging.LogScreenDisposed(_logger);

            _disposed = true;
        }

        Sdl.Quit();
        SageGameLogging.LogSdlQuit(_logger);

        CommonLogging.LogDisposed(_logger, nameof(SageGame));
    }

    private void ParseEvents()
    {
        while (Sdl.PollEvent(out Sdl.Event e))
        {
            SageGameLogging.LogEventPolled(_logger, e.Type.ToString());

            if (e.Type == Sdl.EventType.Quit)
            {
                SageGameLogging.LogQuitEventReceived(_logger);
                _isRunning = false;
            }
        }
    }

    private void Update()
    {
        SageGameLogging.LogUpdateStarted(_logger);

        ParseEvents();
        _gameTime.Update();

        // Log after update so callers can see post-update timings
        SageGameLogging.LogUpdateFinished(
            _logger,
            _gameTime.TotalTime.TotalMilliseconds,
            _gameTime.DeltaTime.TotalMilliseconds
        );
    }

    private void Draw()
    {
        SageGameLogging.LogDrawStarted(_logger);

        _screen.Draw();

        SageGameLogging.LogDrawFinished(
            _logger,
            _gameTime.TotalTime.TotalMilliseconds,
            _gameTime.DeltaTime.TotalMilliseconds
        );
    }

    private void LogOperatingSystemInformation()
    {
        SageGameLogging.LogOperatingSystem(_logger, Environment.OSVersion.VersionString);
        SageGameLogging.LogOSVersion(_logger, Environment.OSVersion.ToString());
        SageGameLogging.LogArchitecture(_logger, RuntimeInformation.OSArchitecture.ToString());
        SageGameLogging.LogProcessArchitecture(_logger, RuntimeInformation.ProcessArchitecture.ToString());
    }

    private void LogRuntimeInformation()
    {
        SageGameLogging.LogFramework(_logger, RuntimeInformation.FrameworkDescription);
        SageGameLogging.LogRuntimeVersion(_logger, Environment.Version.ToString());
        SageGameLogging.LogCLRVersion(_logger, RuntimeInformation.RuntimeIdentifier);
    }

    private void LogHardwareInformation()
    {
        SageGameLogging.LogProcessorCount(_logger, Environment.ProcessorCount);
        SageGameLogging.LogIs64BitProcess(_logger, Environment.Is64BitProcess);
        SageGameLogging.LogIs64BitOS(_logger, Environment.Is64BitOperatingSystem);
    }

    private void LogUserInformation()
    {
        SageGameLogging.LogMachineName(_logger, Environment.MachineName);
        SageGameLogging.LogUserName(_logger, Environment.UserName);
        SageGameLogging.LogUserDomain(_logger, Environment.UserDomainName);
    }

    private void LogCoreSystemDirectories()
    {
        SageGameLogging.LogSystemDirectory(_logger, Environment.SystemDirectory);
        SageGameLogging.LogWorkingDirectory(_logger, Environment.CurrentDirectory);
        SageGameLogging.LogTemporaryDirectory(_logger, Path.GetTempPath());
    }

    private void LogApplicationDataDirectories()
    {
        SageGameLogging.LogApplicationDataDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        );

        SageGameLogging.LogLocalApplicationDataDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        );

        SageGameLogging.LogCommonApplicationDataDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        );
    }

    private void LogProgramDirectories()
    {
        SageGameLogging.LogProgramFilesDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        );

        SageGameLogging.LogProgramFilesX86Directory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        );

        SageGameLogging.LogCommonProgramFilesDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles)
        );

        SageGameLogging.LogCommonProgramFilesX86Directory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86)
        );
    }

    private void LogUserDirectories()
    {
        SageGameLogging.LogUserProfileDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        );

        SageGameLogging.LogDesktopDirectory(_logger, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        SageGameLogging.LogDocumentsDirectory(_logger, Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        SageGameLogging.LogMyDocumentsDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        );

        SageGameLogging.LogMyMusicDirectory(_logger, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        SageGameLogging.LogMyPicturesDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        );

        SageGameLogging.LogMyVideosDirectory(_logger, Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
    }

    private void LogSystemMenuDirectories()
    {
        SageGameLogging.LogStartupDirectory(_logger, Environment.GetFolderPath(Environment.SpecialFolder.Startup));
        SageGameLogging.LogStartMenuDirectory(_logger, Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
        SageGameLogging.LogCommonStartMenuDirectory(
            _logger,
            Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)
        );
    }

    private void LogDirectoryInformation()
    {
        LogCoreSystemDirectories();
        LogApplicationDataDirectories();
        LogProgramDirectories();
        LogUserDirectories();
        LogSystemMenuDirectories();
    }

    private void LogProcessInformation()
    {
        using var currentProcess = Process.GetCurrentProcess();
        SageGameLogging.LogCurrentProcess(_logger, currentProcess.ProcessName, currentProcess.Id);
        SageGameLogging.LogProcessStartTime(_logger, currentProcess.StartTime);
    }

    private void LogMemoryInformation()
    {
        var gcMemory = GC.GetTotalMemory(false);
        SageGameLogging.LogGCMemory(_logger, gcMemory / (1024 * 1024));
        SageGameLogging.LogTotalPhysicalMemory(_logger, Sdl.GetSystemRam());
    }

    private void LogSystemInformation()
    {
        LogOperatingSystemInformation();
        LogRuntimeInformation();
        LogHardwareInformation();
        LogMemoryInformation();
        LogProcessInformation();
        LogUserInformation();
        LogDirectoryInformation();
    }
}
