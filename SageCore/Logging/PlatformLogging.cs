// -----------------------------------------------------------------------
// <copyright file="PlatformLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using SageCore.NativeMethods.Sdl3;

namespace SageCore.Logging;

internal static partial class PlatformLogging
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to set the app data '{Data}' to '{Value}'")]
    public static partial void LogFailedToSetAppData(ILogger logger, string data, string value);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Successfully set app data '{Data}' to '{Value}'")]
    public static partial void LogSuccessfullySetAppData(ILogger logger, string data, string value);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Initializing app options")]
    public static partial void LogInitializingAppOptions(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "App options initialized")]
    public static partial void LogAppOptionsInitialized(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Initializing SDL logging")]
    public static partial void LogInitializingSdlLogging(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "SDL log priority set to {priority}")]
    public static partial void LogSdlLogPrioritySet(ILogger logger, Sdl.LogPriority priority);

    [LoggerMessage(Level = LogLevel.Debug, Message = "SDL logging initialized")]
    public static partial void LogSdlLoggingInitialized(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "SDL log output function handle freed")]
    public static partial void LogSdlLogOutputFunctionFreed(ILogger logger);
}
