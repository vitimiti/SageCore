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
    /// <summary>
    /// Initializes a new instance of the <see cref="Screen"/> class.
    /// </summary>
    /// <param name="options">The screen options to configure the screen. If null, default options will be used.</param>
    /// <exception cref="InvalidOperationException">Thrown if the SDL video subsystem fails to initialize.</exception>
    public Screen(ScreenOptions? options)
    {
        if (!Sdl.Init(Sdl.InitVideo))
        {
            throw new InvalidOperationException($"Failed to initialize the SDL video subsystem: {Sdl.GetError()}.");
        }
    }

    /// <inheritdoc/>
    public void Dispose() => Sdl.QuitSubSystem(Sdl.InitVideo);
}
