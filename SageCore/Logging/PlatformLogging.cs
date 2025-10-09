// -----------------------------------------------------------------------
// <copyright file="PlatformLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class PlatformLogging
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to set the app data '{Data}' to '{Value}'")]
    public static partial void LogFailedToSetAppData(ILogger logger, string Data, string Value);
}
