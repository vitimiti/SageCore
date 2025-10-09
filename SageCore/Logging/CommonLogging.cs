// -----------------------------------------------------------------------
// <copyright file="CommonLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class CommonLogging
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "{SystemName} initializing")]
    public static partial void LogInitializing(ILogger logger, string systemName);

    [LoggerMessage(Level = LogLevel.Information, Message = "{SystemName} initialized successfully")]
    public static partial void LogInitialized(ILogger logger, string systemName);

    [LoggerMessage(Level = LogLevel.Debug, Message = "{SystemName} disposing")]
    public static partial void LogDisposing(ILogger logger, string systemName);

    [LoggerMessage(Level = LogLevel.Information, Message = "{SystemName} disposed")]
    public static partial void LogDisposed(ILogger logger, string systemName);
}
