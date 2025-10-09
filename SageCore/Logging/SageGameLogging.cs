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
    public static partial void LogGameLoopIteration(ILogger logger, double TotalTimeMs, double DeltaTimeMs);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Platform system disposed")]
    public static partial void LogPlatformSystemDisposed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Screen disposed")]
    public static partial void LogScreenDisposed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "SDL quit called")]
    public static partial void LogSdlQuit(ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Attempted to run game that has already been disposed")]
    public static partial void LogRunOnDisposedGame(ILogger logger);
}
