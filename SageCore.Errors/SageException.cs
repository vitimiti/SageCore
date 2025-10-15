// -----------------------------------------------------------------------
// <copyright file="SageException.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Errors;

/// <summary>
/// The base exception type for all exceptions thrown by SageCore.
/// </summary>
public class SageException : Exception
{
    internal const uint BaseErrorCode = 0xDE_AD_00_00;

    /// <summary>
    /// Initializes a new instance of the <see cref="SageException"/> class.
    /// </summary>
    public SageException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SageException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SageException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Gets the error code associated with this exception.
    /// </summary>
    public uint Code => ErrorCode;

    /// <summary>
    /// Gets the error code associated with this exception.
    /// </summary>
    protected virtual uint ErrorCode { get; } = BaseErrorCode;
}
