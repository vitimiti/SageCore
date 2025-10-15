// -----------------------------------------------------------------------
// <copyright file="XferStatus.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Common.Data;

/// <summary>
/// Specifies the status of a file transfer operation.
/// </summary>
public enum XferStatus
{
    /// <summary>
    /// The transfer status is invalid or uninitialized.
    /// </summary>
    Invalid,

    /// <summary>
    /// The transfer completed successfully.
    /// </summary>
    Ok,

    /// <summary>
    /// The end of the file was reached during the transfer.
    /// </summary>
    Eof,

    /// <summary>
    /// The file was not found.
    /// </summary>
    FileNotFound,

    /// <summary>
    /// The file is not open.
    /// </summary>
    FileNotOpen,

    /// <summary>
    /// The file is already open.
    /// </summary>
    FileAlreadyOpen,

    /// <summary>
    /// An error occurred while reading the file.
    /// </summary>
    ReadError,

    /// <summary>
    /// An error occurred while writing to the file.
    /// </summary>
    WriteError,

    /// <summary>
    /// The file mode is unknown or unsupported.
    /// </summary>
    ModeUnknown,

    /// <summary>
    /// An error occurred while skipping data in the file.
    /// </summary>
    SkipError,

    /// <summary>
    /// The begin and end markers do not match.
    /// </summary>
    BeginEndMismatch,

    /// <summary>
    /// An error occurred while seeking within the file.
    /// </summary>
    OutOfMemory,

    /// <summary>
    /// An error occurred due to an invalid string.
    /// </summary>
    StringError,

    /// <summary>
    /// The version is invalid.
    /// </summary>
    InvalidVersion,

    /// <summary>
    /// The parameters provided are invalid.
    /// </summary>
    InvalidParameters,

    /// <summary>
    /// The list is not empty when it was expected to be.
    /// </summary>
    ListNotEmpty,

    /// <summary>
    /// The string is unknown or unrecognized.
    /// </summary>
    UnknownString,

    /// <summary>
    /// An unknown error occurred.
    /// </summary>
    UnknownError,
}
