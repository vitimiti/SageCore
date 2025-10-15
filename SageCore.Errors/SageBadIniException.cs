// -----------------------------------------------------------------------
// <copyright file="SageBadIniException.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Errors;

/// <summary>
/// Exception thrown when an INI file is malformed or cannot be parsed.
/// </summary>
public class SageBadIniException : SageException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SageBadIniException"/> class.
    /// </summary>
    public SageBadIniException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageBadIniException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SageBadIniException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageBadIniException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SageBadIniException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Gets the error code associated with this exception.
    /// </summary>
    protected override uint ErrorCode { get; } = BaseErrorCode + 0x00_05;
}
