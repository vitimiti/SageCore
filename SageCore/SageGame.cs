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
using Microsoft.Extensions.Options;
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
    public SageGame([NotNull] ILogger logger, IOptions<SageGameOptions>? options = null)
    {
        _logger = logger;
        _options = options?.Value ?? new SageGameOptions();

        LogSystemInformation();

        CommonLogging.LogInitializing(_logger, nameof(SageGame));

        _gameTime = new GameTime(_logger);
        _platformSystem = new PlatformSystem(_logger, _options.AppOptions);
        _screen = new Screen(_options.ScreenOptions);

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
            _gameTime.Update();

            // Log each iteration at debug level (can be useful for debugging but not too verbose)
            SageGameLogging.LogGameLoopIteration(
                _logger,
                _gameTime.TotalTime.TotalMilliseconds,
                _gameTime.DeltaTime.TotalMilliseconds
            );

            // Game update and rendering logic would go here.
            _isRunning = false; // For demonstration, we stop after one loop.
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

    private void LogSystemInformation()
    {
        SageGameLogging.LogSystemInfoHeader(_logger);

        // Operating System Information
        SageGameLogging.LogOperatingSystem(_logger, Environment.OSVersion.VersionString);
        SageGameLogging.LogOSVersion(_logger, Environment.OSVersion.ToString());
        SageGameLogging.LogArchitecture(_logger, RuntimeInformation.OSArchitecture.ToString());
        SageGameLogging.LogProcessArchitecture(_logger, RuntimeInformation.ProcessArchitecture.ToString());

        // Runtime Information
        SageGameLogging.LogFramework(_logger, RuntimeInformation.FrameworkDescription);
        SageGameLogging.LogRuntimeVersion(_logger, Environment.Version.ToString());
        SageGameLogging.LogCLRVersion(_logger, RuntimeInformation.RuntimeIdentifier);

        // Hardware Information
        SageGameLogging.LogProcessorCount(_logger, Environment.ProcessorCount);
        SageGameLogging.LogIs64BitProcess(_logger, Environment.Is64BitProcess);
        SageGameLogging.LogIs64BitOS(_logger, Environment.Is64BitOperatingSystem);

        // System Information
        SageGameLogging.LogMachineName(_logger, Environment.MachineName);
        SageGameLogging.LogUserName(_logger, Environment.UserName);
        SageGameLogging.LogUserDomain(_logger, Environment.UserDomainName);
        SageGameLogging.LogWorkingDirectory(_logger, Environment.CurrentDirectory);
        SageGameLogging.LogSystemDirectory(_logger, Environment.SystemDirectory);

        // Process Information
        using var currentProcess = Process.GetCurrentProcess();
        SageGameLogging.LogCurrentProcess(_logger, currentProcess.ProcessName, currentProcess.Id);
        SageGameLogging.LogProcessStartTime(_logger, currentProcess.StartTime);

        // Memory Information
        var gcMemory = GC.GetTotalMemory(false);
        SageGameLogging.LogGCMemory(_logger, gcMemory / (1024 * 1024));

        // Get system memory information (cross-platform approach)
        try
        {
            GCMemoryInfo memoryInfo = GC.GetGCMemoryInfo();
            if (memoryInfo.TotalAvailableMemoryBytes > 0)
            {
                SageGameLogging.LogTotalPhysicalMemory(_logger, memoryInfo.TotalAvailableMemoryBytes / (1024 * 1024));
            }
        }
        catch (PlatformNotSupportedException)
        {
            // GC memory info may not be available on all platforms
        }
        catch (NotSupportedException)
        {
            // GC memory info may not be supported in this runtime version
        }

        SageGameLogging.LogSystemInfoFooter(_logger);
    }
}
