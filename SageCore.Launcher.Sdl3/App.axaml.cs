// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SageCore.Abstractions;
using SageCore.Launcher.Sdl3.ViewModels;
using SageCore.Launcher.Sdl3.Views;
using SageCore.Platform.Sdl3;

namespace SageCore.Launcher.Sdl3;

internal sealed partial class App : Application
{
    private IHost? _host;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        _host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            MainWindow mainWindow = new();

            // Get the view model from DI and pass the window to it
            MainWindowViewModel viewModel = _host.Services.GetRequiredService<MainWindowViewModel>();
            viewModel.SetWindow(mainWindow);

            mainWindow.DataContext = viewModel;
            desktop.MainWindow = mainWindow;

            desktop.Exit += (_, _) => _host?.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddSingleton<IMessageBox, MessageBox>();

        _ = services.AddSingleton<MainWindowViewModel>();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove =
        [
            .. BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>(),
        ];

        // remove each entry found
        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            _ = BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
