// -----------------------------------------------------------------------
// <copyright file="SageGame.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private readonly PlatformSystem _platformSystem;
    private readonly GameTime _gameTime = new();
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

        _platformSystem = new PlatformSystem(_logger, _options.AppOptions);
        _screen = new Screen(_options.ScreenOptions);
    }

    /// <summary>
    /// Runs the game instance.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the game instance has been disposed.</exception>
    public void Run()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _isRunning = true;
        while (_isRunning)
        {
            _gameTime.Update();

            // Game update and rendering logic would go here.
            _isRunning = false; // For demonstration, we stop after one loop.
        }
    }

    /// <summary>
    /// Disposes of the game instance.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _platformSystem.Dispose();
            _screen.Dispose();

            _disposed = true;
        }

        Sdl.Quit();
    }
}
