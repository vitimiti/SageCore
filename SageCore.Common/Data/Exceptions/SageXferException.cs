// -----------------------------------------------------------------------
// <copyright file="SageXferException.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Errors;

namespace SageCore.Common.Data.Exceptions;

/// <summary>
/// Exception thrown for errors during data transfer operations.
/// </summary>
public class SageXferException : SageException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SageXferException"/> class.
    /// </summary>
    public SageXferException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageXferException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SageXferException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageXferException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SageXferException(string message, Exception innerException)
        : base(message, innerException) { }
}
