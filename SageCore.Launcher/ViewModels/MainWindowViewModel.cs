// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Microsoft.Extensions.Logging;
using SageCore.Utilities;

namespace SageCore.Launcher.ViewModels;

internal sealed partial class MainWindowViewModel : ViewModelBase
{
    private readonly Window _mainWindow;

    public string Greeting { get; } = "Welcome to Avalonia!";

    public MainWindowViewModel([NotNull] Window mainWindow)
    {
        _mainWindow = mainWindow;

        StartSageGame();
    }

    [SuppressMessage(
        "Design",
        "CA1031: Do not catch general exception types",
        Justification = "Catching all exceptions to show an error message."
    )]
    private void StartSageGame()
    {
        _mainWindow.WindowState = WindowState.Minimized;

        // TODO: Add configuration for logging
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            builder.AddConsole().SetMinimumLevel(LogLevel.Trace)
        );

        ILogger<SageGame> logger = loggerFactory.CreateLogger<SageGame>();

        try
        {
            using SageGame sageGame = new(
                logger,
                (options) =>
                {
                    options.UseExpansionEngine = false; // Test the base game first!
                    options.ScreenOptions.FullScreen = false;
                }
            );

            GameLogging.LogGameInstanceCreated(logger);
            sageGame.Run();
            GameLogging.LogGameRunEnded(logger);
        }
        catch (Exception ex)
        {
            MessageBox.ShowError("An error occurred while running the game.", $"{ex}");
            GameLogging.LogGameRunError(logger, ex.ToString());
        }
        finally
        {
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }

        GameLogging.LogGameInstanceDisposed(logger);
    }

    // TODO: Add games names and info to the logging
    private static partial class GameLogging
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Game instance created.")]
        public static partial void LogGameInstanceCreated(ILogger logger);

        [LoggerMessage(Level = LogLevel.Information, Message = "Game instance disposed.")]
        public static partial void LogGameInstanceDisposed(ILogger logger);

        [LoggerMessage(Level = LogLevel.Information, Message = "Game run ended.")]
        public static partial void LogGameRunEnded(ILogger logger);

        [LoggerMessage(Level = LogLevel.Error, Message = "An error occurred while running the game: {ErrorMessage}")]
        public static partial void LogGameRunError(ILogger logger, string errorMessage);
    }
}
