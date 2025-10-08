// -----------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Abstractions;
using SageCore.Platform.Sdl3.NativeMethods;

namespace SageCore.Platform.Sdl3;

/// <summary>
/// An implementation of the <see cref="IMessageBox"/> interface using SDL3.
/// </summary>
public sealed class MessageBox : IMessageBox
{
    /// <inheritdoc/>
    public void ShowError(string message, string title)
    {
        if (
            !Sdl.ShowSimpleMessageBox(Sdl.MessageBoxError | Sdl.MessageBoxButtonsLeftToRight, title, message, nint.Zero)
        )
        {
            Console.WriteLine($"(SDL3 MessageBox failed: {Sdl.GetError()}) Error: {title}\n{message}");
        }
    }
}
