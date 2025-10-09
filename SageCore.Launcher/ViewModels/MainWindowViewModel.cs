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

        try
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<SageGame> logger = loggerFactory.CreateLogger<SageGame>();
            using SageGame sageGame = new(logger);
            sageGame.Run();
        }
        catch (Exception ex)
        {
            MessageBox.ShowError("An error occurred while running the game.", $"{ex}");
        }
        finally
        {
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }
    }
}
