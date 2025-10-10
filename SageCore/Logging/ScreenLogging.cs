// -----------------------------------------------------------------------
// <copyright file="ScreenLogging.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Logging;

internal static partial class ScreenLogging
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "GPU device claimed window successfully")]
    public static partial void LogWindowClaimed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "GPU device created")]
    public static partial void LogGpuDeviceCreated(ILogger logger);

    [LoggerMessage(Level = LogLevel.Debug, Message = "GPU device released window")]
    public static partial void LogGpuDeviceReleasedWindow(ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Window created: {Title} ({Width}x{Height}) FullScreen: {FullScreen})"
    )]
    public static partial void LogWindowCreated(ILogger logger, string title, int width, int height, bool fullScreen);

    [LoggerMessage(Level = LogLevel.Warning, Message = "GPU command buffer invalid for this frame - skipping draw")]
    public static partial void LogCommandBufferInvalid(ILogger logger);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Failed to acquire swapchain texture for window - skipping draw"
    )]
    public static partial void LogSwapchainAcquireFailed(ILogger logger);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Render pass invalid - skipping draw")]
    public static partial void LogRenderPassInvalid(ILogger logger);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to submit GPU command buffer")]
    public static partial void LogCommandBufferSubmitFailed(ILogger logger);
}
