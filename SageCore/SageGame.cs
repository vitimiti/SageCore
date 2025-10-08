// -----------------------------------------------------------------------
// <copyright file="SageGame.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SageCore;

/// <summary>
/// A basic game class to represent a Sage game instance.
/// </summary>
/// <param name="logger">The logger instance for logging game events.</param>
[DebuggerDisplay("IsRunning = {IsRunning}")]
public sealed class SageGame(ILogger logger) : IDisposable
{
    private readonly GameTime _gameTime = new();

    /// <summary>
    /// Gets or sets a value indicating whether the game is currently running.
    /// </summary>
    private bool IsRunning { get; set; }

    /// <summary>
    /// Runs the game instance.
    /// </summary>
    public void Run()
    {
        IsRunning = true;
        while (IsRunning)
        {
            _gameTime.Update();

            // Game update and rendering logic would go here.
            IsRunning = false; // For demonstration, we stop after one loop.
        }
    }

    /// <summary>
    /// Disposes of the game instance.
    /// </summary>
    public void Dispose() { }
}
