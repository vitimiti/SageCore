// -----------------------------------------------------------------------
// <copyright file="Screen.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SageCore.Logging;
using SageCore.NativeMethods.Sdl3;
using SageCore.Options;

namespace SageCore.Systems.VideoSystem;

/// <summary>
/// Represents the screen to display video content.
/// </summary>
/// <remarks>This class failing to initialize will throw exceptions instead of stopping the system, as you can't play without a screen.</remarks>
internal sealed class Screen : IDisposable
{
    private readonly ILogger _logger;
    private readonly ScreenOptions _options;
    private readonly Sdl.WindowSafeHandle _window = new();
    private readonly Sdl.GpuDeviceSafeHandle _gpuDevice = new();

    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="Screen"/> class.
    /// </summary>
    /// <param name="logger">The logger to use for logging.</param>
    /// <param name="options">The screen options to configure the screen. If null, default options will be used.</param>
    /// <exception cref="InvalidOperationException">Thrown if the SDL video subsystem or GPU device fails to initialize.</exception>
    public Screen([NotNull] ILogger logger, ScreenOptions? options)
    {
        _logger = logger;
        _options = options ?? new ScreenOptions();

        CommonLogging.LogInitializing(_logger, nameof(Screen));

        if (!Sdl.Init(Sdl.InitVideo))
        {
            throw new InvalidOperationException($"Failed to initialize the SDL video subsystem: {Sdl.GetError()}.");
        }

#if DEBUG
        const bool debugMode = true;
#else
        const bool debugMode = false;
#endif

        _gpuDevice = Sdl.GpuDeviceSafeHandle.Create(
            Sdl.GpuShaderFormatSpirv | Sdl.GpuShaderFormatDxil | Sdl.GpuShaderFormatMsl,
            debugMode,
            name: null
        );

        if (_gpuDevice.IsInvalid)
        {
            throw new InvalidOperationException($"Failed to create the GPU device: {Sdl.GetError()}.");
        }

        ScreenLogging.LogGpuDeviceCreated(_logger);

        var windowFlags = Sdl.WindowResizable | Sdl.WindowMouseCapture;
        if (_options.FullScreen)
        {
            windowFlags |= Sdl.WindowFullscreen;
        }

        _window = Sdl.WindowSafeHandle.Create(_options.Title, _options.Width, _options.Height, windowFlags);
        if (_window.IsInvalid)
        {
            throw new InvalidOperationException($"Failed to create the SDL window: {Sdl.GetError()}.");
        }

        ScreenLogging.LogWindowCreated(_logger, _options.Title, _options.Width, _options.Height, _options.FullScreen);

        if (!_gpuDevice.TryClaimWindow(_window))
        {
            throw new InvalidOperationException($"Failed to claim the window for the GPU device: {Sdl.GetError()}.");
        }

        ScreenLogging.LogWindowClaimed(_logger);
        CommonLogging.LogInitialized(_logger, nameof(Screen));
    }

    /// <summary>
    /// Draws the current frame to the screen.
    /// </summary>
    /// <remarks>This method should be called once per frame.</remarks>
    /// <exception cref="ObjectDisposedException">Thrown if the screen has been disposed.</exception>
    public void Draw()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Screen));

        using var commandBuffer = Sdl.GpuCommandBufferSafeHandle.Acquire(_gpuDevice);
        if (commandBuffer.IsInvalid)
        {
            // Log and return, but don't throw - we can try again next frame
            ScreenLogging.LogCommandBufferInvalid(_logger);
            return;
        }

        if (
            !commandBuffer.TryWaitAndAcquireSwapchainTexture(
                _window,
                out Sdl.GpuTextureSafeHandle swapchainTexture,
                out _,
                out _
            )
        )
        {
            // Log and return, but don't throw - we can try again next frame
            ScreenLogging.LogSwapchainAcquireFailed(_logger);
            swapchainTexture.Dispose();
            return;
        }

        if (!swapchainTexture.IsInvalid)
        {
            var referenced = false;
            swapchainTexture.DangerousAddRef(ref referenced);
            try
            {
                Sdl.GpuColorTargetInfo colorTargetInfo = new()
                {
                    Texture = swapchainTexture.DangerousGetHandle(),
                    ClearColor = new Sdl.FColor
                    {
                        R = 0F,
                        G = 0F,
                        B = 0F,
                        A = 1F,
                    },
                    LoadOp = Sdl.GpuLoadOp.Clear,
                    StoreOp = Sdl.GpuStoreOp.Store,
                };

                using Sdl.GpuRenderPassSafeHandle renderPass = commandBuffer.BeginRenderPass([colorTargetInfo]);
                if (renderPass.IsInvalid)
                {
                    // Log and continue, but don't throw - we can try again next frame
                    ScreenLogging.LogRenderPassInvalid(_logger);
                }

                renderPass.End();
            }
            finally
            {
                if (referenced)
                {
                    swapchainTexture.DangerousRelease();
                }
            }
        }

        if (!commandBuffer.TrySubmit())
        {
            // Log and continue, but don't throw - we can try again next frame
            ScreenLogging.LogCommandBufferSubmitFailed(_logger);
        }

        swapchainTexture.Dispose();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        CommonLogging.LogDisposing(_logger, nameof(Screen));

        if (!_gpuDevice.IsInvalid)
        {
            _gpuDevice.ReleaseWindow(_window);
            ScreenLogging.LogGpuDeviceReleasedWindow(_logger);
        }

        _gpuDevice.Dispose();
        _window.Dispose();

        Sdl.QuitSubSystem(Sdl.InitVideo);
        _disposed = true;

        CommonLogging.LogDisposed(_logger, nameof(Screen));
    }
}
