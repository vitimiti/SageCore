// -----------------------------------------------------------------------
// <copyright file="MainWindow.axaml.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;

namespace SageCore.Launcher.Views;

[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Used by Avalonia")]
internal sealed partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
}
