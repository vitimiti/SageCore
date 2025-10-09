// -----------------------------------------------------------------------
// <copyright file="Screen.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.NativeMethods.Sdl3;
using SageCore.Options;

namespace SageCore.Systems.VideoSystem;

/// <summary>
/// Represents the screen to display video content.
/// </summary>
/// <remarks>This class failing to initialize will throw exceptions instead of stopping the system, as you can't play without a screen.</remarks>
internal sealed class Screen : IDisposable
{
    private readonly ScreenOptions _options;
    private readonly Sdl.WindowSafeHandle _window = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Screen"/> class.
    /// </summary>
    /// <param name="options">The screen options to configure the screen. If null, default options will be used.</param>
    /// <exception cref="InvalidOperationException">Thrown if the SDL video subsystem fails to initialize.</exception>
    public Screen(ScreenOptions? options)
    {
        _options = options ?? new ScreenOptions();

        if (!Sdl.Init(Sdl.InitVideo))
        {
            throw new InvalidOperationException($"Failed to initialize the SDL video subsystem: {Sdl.GetError()}.");
        }

        var windowFlags = Sdl.WindowResizable | Sdl.WindowMouseCapture;
        if (_options.FullScreen)
        {
            windowFlags |= Sdl.WindowFullscreen;
        }

        _window = Sdl.WindowSafeHandle.Create(_options.Title, _options.Width, _options.Height, windowFlags);
        if (_window.IsInvalid)
        {
            throw new InvalidOperationException($"Failed to create the SDL window: {Sdl.GetError()}.");
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (!_window.IsInvalid)
        {
            _window.Dispose();
        }

        Sdl.QuitSubSystem(Sdl.InitVideo);
    }
}
