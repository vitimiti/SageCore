// -----------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.NativeMethods.Sdl3;

namespace SageCore.Utils;

/// <summary>
/// A simple message box implementation using SDL3.
/// </summary>
public sealed class MessageBox
{
    /// <summary>
    /// Shows an error message box with the specified message and title.
    /// </summary>
    /// <param name="title">The title of the message box.</param>
    /// <param name="message">The message to display.</param>
    /// <remarks>This method is not thread-safe.</remarks>
    public static void ShowError(string title, string message)
    {
        if (
            !Sdl.TryShowSimpleMessageBox(
                Sdl.MessageBoxError | Sdl.MessageBoxButtonsLeftToRight,
                title,
                message,
                nint.Zero
            )
        )
        {
            Console.WriteLine($"(SDL3 MessageBox failed: {Sdl.GetError()})\nError: {title}\n{message}");
        }
    }
}
