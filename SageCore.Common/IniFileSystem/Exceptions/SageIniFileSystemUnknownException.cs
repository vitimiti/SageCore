// -----------------------------------------------------------------------
// <copyright file="SageIniFileSystemUnknownException.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Errors;

namespace SageCore.Common.IniFileSystem.Exceptions;

/// <summary>
/// Exception thrown for unknown INI file system errors.
/// </summary>
public class SageIniFileSystemUnknownException : SageBadIniException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SageIniFileSystemUnknownException"/> class.
    /// </summary>
    public SageIniFileSystemUnknownException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageIniFileSystemUnknownException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SageIniFileSystemUnknownException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SageIniFileSystemUnknownException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public SageIniFileSystemUnknownException(string? message, Exception? inner)
        : base(message, inner) { }
}
