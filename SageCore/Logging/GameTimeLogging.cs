// -----------------------------------------------------------------------
// <copyright file="GameTimeLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class GameTimeLogging
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "GameTime initialized with tick ratio {TickRatio}")]
    public static partial void LogGameTimeInitialized(ILogger logger, double tickRatio);

    [LoggerMessage(Level = LogLevel.Debug, Message = "GameTime reset - new start time: {StartTime} ticks")]
    public static partial void LogGameTimeReset(ILogger logger, long startTime);

    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "GameTime updated - Total: {TotalTimeMs}ms, Delta: {DeltaTimeMs}ms"
    )]
    public static partial void LogGameTimeUpdated(ILogger logger, double totalTimeMs, double deltaTimeMs);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Delta time clamped from {OriginalDeltaMs}ms to {ClampedDeltaMs}ms to prevent large time jumps"
    )]
    public static partial void LogDeltaTimeClamped(ILogger logger, double originalDeltaMs, double clampedDeltaMs);

    [LoggerMessage(Level = LogLevel.Information, Message = "GameTime stopwatch started")]
    public static partial void LogStopwatchStarted(ILogger logger);
}
