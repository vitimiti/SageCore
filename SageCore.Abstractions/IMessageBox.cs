// -----------------------------------------------------------------------
// <copyright file="IMessageBox.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Abstractions;

/// <summary>
/// An interface representing a message box service for displaying messages to the user.
/// </summary>
public interface IMessageBox
{
    /// <summary>
    /// Shows an error message box with the specified message and title.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    /// <param name="title">The title of the error message box.</param>
    void ShowError(string message, string title);
}
