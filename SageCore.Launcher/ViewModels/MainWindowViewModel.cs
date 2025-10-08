// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Microsoft.Extensions.Logging;

namespace SageCore.Launcher.ViewModels;

internal sealed partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger _sageGameLogger;
    private readonly Window _mainWindow;

    public string Greeting { get; } = "Welcome to Avalonia!";

    public MainWindowViewModel([NotNull] Window mainWindow)
    {
        _mainWindow = mainWindow;

        // TODO: Allow configuration of logging level and output (e.g., file, console, etc.)
        // For now, we will log to the console with a default log level.
#if DEBUG
        const LogLevel logLevel = LogLevel.Debug;
#else
        const LogLevel logLevel = LogLevel.Information;
#endif

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            builder.AddConsole().SetMinimumLevel(logLevel)
        );

        _sageGameLogger = loggerFactory.CreateLogger<SageGame>();

        // Just run a default game instance for now.
        // TODO: Add options to control the type of game and where the game files are located.
        StartSageGame();
    }

    public void StartSageGame()
    {
        _mainWindow.WindowState = WindowState.Minimized;

        try
        {
            using SageGame sageGame = new(_sageGameLogger);
            sageGame.Run();
        }
        finally
        {
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }

        // TODO: Catch the exception and show a user-friendly error message in the UI.
    }
}
