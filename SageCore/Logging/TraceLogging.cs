// -----------------------------------------------------------------------
// <copyright file="TraceLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class TraceLogging
{
    [LoggerMessage(
        Level = LogLevel.Trace,
        Message = "Total game time: {TotalTime} (+{DeltaTime}) @ {TickRatio} ticks/stopwatch tick"
    )]
    public static partial void LogGameTime(ILogger logger, TimeSpan totalTime, TimeSpan deltaTime, double tickRatio);
}
