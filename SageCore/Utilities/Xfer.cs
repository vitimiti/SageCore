// -----------------------------------------------------------------------
// <copyright file="Xfer.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Utilities;

internal abstract class Xfer
{
    /// <summary>
    /// Handles user-defined data processing.
    /// </summary>
    /// <param name="data">The data to be processed.</param>
    /// <remarks>This method must be implemented by derived classes to define specific data handling behavior.</remarks>
    public abstract void User(Span<byte> data);
}
