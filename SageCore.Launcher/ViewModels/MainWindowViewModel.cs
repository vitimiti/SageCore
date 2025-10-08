// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace SageCore.Launcher.ViewModels;

internal sealed partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger _logger;

    public string Greeting { get; } = "Welcome to Avalonia!";

    public MainWindowViewModel()
    {
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

        _logger = loggerFactory.CreateLogger<SageGame>();

        // Just run a default game instance for now.
        // TODO: Add options to control the type of game and where the game files are located.
        StartSageGame();
    }

    public void StartSageGame()
    {
        using SageGame sageGame = new(_logger);
        sageGame.Run();
    }
}
