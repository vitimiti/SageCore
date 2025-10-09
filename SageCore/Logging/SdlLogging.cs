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
    public static partial void Trace(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Debug, Message = "[{Category}] {Message}")]
    public static partial void Debug(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Information, Message = "[{Category}] {Message}")]
    public static partial void Information(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Warning, Message = "[{Category}] {Message}")]
    public static partial void Warning(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Error, Message = "[{Category}] {Message}")]
    public static partial void Error(ILogger logger, string Category, string Message);

    [LoggerMessage(Level = LogLevel.Critical, Message = "[{Category}] {Message}")]
    public static partial void Critical(ILogger logger, string Category, string Message);
}
