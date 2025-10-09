// -----------------------------------------------------------------------
// <copyright file="SdlLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class SdlLogging
{
    [LoggerMessage(Level = LogLevel.Trace, Message = "[{Category}] {Message}")]
    public static partial void LogTrace(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Debug, Message = "[{Category}] {Message}")]
    public static partial void LogDebug(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Information, Message = "[{Category}] {Message}")]
    public static partial void LogInformation(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Warning, Message = "[{Category}] {Message}")]
    public static partial void LogWarning(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Error, Message = "[{Category}] {Message}")]
    public static partial void LogError(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Critical, Message = "[{Category}] {Message}")]
    public static partial void LogCritical(ILogger logger, string Category, string Message);
}
