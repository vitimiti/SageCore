// -----------------------------------------------------------------------
// <copyright file="SageGame.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore;

/// <summary>
/// A basic game class to represent a Sage game instance.
/// </summary>
/// <param name="logger">The logger instance to use for logging.</param>
public sealed class SageGame(ILogger logger) : IDisposable
{
    /// <summary>
    /// Runs the game instance.
    /// </summary>
    public void Run() { }

    /// <summary>
    /// Disposes of the game instance.
    /// </summary>
    public void Dispose() { }
}
