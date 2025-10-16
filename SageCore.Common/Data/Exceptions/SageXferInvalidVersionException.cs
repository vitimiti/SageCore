// -----------------------------------------------------------------------
// <copyright file="SageXferInvalidVersionException.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Common.Data.Exceptions;

/// <summary>
/// Exception thrown when a data transfer operation encounters an invalid version.
/// </summary>
public class SageXferInvalidVersionException : SageXferException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SageXferInvalidVersionException"/> class.
    /// </summary>
    public SageXferInvalidVersionException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageXferInvalidVersionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SageXferInvalidVersionException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageXferInvalidVersionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SageXferInvalidVersionException(string message, Exception innerException)
        : base(message, innerException) { }
}
