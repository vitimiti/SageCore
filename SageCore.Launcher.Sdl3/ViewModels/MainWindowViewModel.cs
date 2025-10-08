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
using SageCore.Abstractions;

namespace SageCore.Launcher.Sdl3.ViewModels;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Used by Avalonia")]
internal sealed partial class MainWindowViewModel([NotNull] IMessageBox messageBox) : ViewModelBase
{
    private Window? _mainWindow;

    public string Greeting { get; } = "Welcome to Avalonia!";

    public void SetWindow([NotNull] Window window)
    {
        _mainWindow = window;
        StartSageGame();
    }

    [SuppressMessage(
        "Design",
        "CA1031: Do not catch general exception types",
        Justification = "Catching all exceptions to show an error message."
    )]
    private void StartSageGame()
    {
        if (_mainWindow is null)
        {
            messageBox.ShowError("Initialization Error", "Main window is not set.");
            return;
        }

        _mainWindow.WindowState = WindowState.Minimized;

        try
        {
            using SageGame sageGame = new();
            sageGame.Run();
        }
        catch (Exception ex)
        {
            messageBox.ShowError("An error occurred while running the game.", $"{ex}");
        }
        finally
        {
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }
    }
}
