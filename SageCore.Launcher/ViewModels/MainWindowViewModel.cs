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
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<MainWindowViewModel>();

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
