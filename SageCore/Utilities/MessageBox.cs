// -----------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.NativeMethods.Sdl3;

namespace SageCore.Utilities;

/// <summary>
/// A simple message box implementation using SDL3.
/// </summary>
public sealed class MessageBox
{
    /// <summary>
    /// Shows an error message box with the specified message and title.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title of the message box.</param>
    /// <remarks>This method is not thread-safe.</remarks>
    public static void ShowError(string message, string title)
    {
        if (
            !Sdl.ShowSimpleMessageBox(Sdl.MessageBoxError | Sdl.MessageBoxButtonsLeftToRight, title, message, nint.Zero)
        )
        {
            Console.WriteLine($"(SDL3 MessageBox failed: {Sdl.GetError()}) Error: {title}\n{message}");
        }
    }
}
