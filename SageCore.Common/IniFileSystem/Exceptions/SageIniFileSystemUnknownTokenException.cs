// -----------------------------------------------------------------------
// <copyright file="SageIniFileSystemUnknownTokenException.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Errors;

namespace SageCore.Common.IniFileSystem.Exceptions;

/// <summary>
/// Exception thrown when an unknown token is encountered in an INI file.
/// </summary>
public class SageIniFileSystemUnknownTokenException : SageBadIniException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SageIniFileSystemUnknownTokenException"/> class.
    /// </summary>
    public SageIniFileSystemUnknownTokenException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageIniFileSystemUnknownTokenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SageIniFileSystemUnknownTokenException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageIniFileSystemUnknownTokenException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SageIniFileSystemUnknownTokenException(string? message, Exception? inner)
        : base(message, inner) { }
}
