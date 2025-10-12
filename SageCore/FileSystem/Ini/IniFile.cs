// -----------------------------------------------------------------------
// <copyright file="IniFile.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SageCore.Extensions;
using SageCore.Utilities;

namespace SageCore.FileSystem.Ini;

/// <summary>
/// Provides functionality to load and parse INI files.
/// </summary>
[DebuggerDisplay("Ini: {_fileName,nq}, LoadType: {_loadType}, Line: {_lineNumber}")]
internal class IniFile : IDisposable
{
    /// <summary>
    /// Delegate for parsing a specific block type in an INI file.
    /// </summary>
    /// <param name="init">The INI instance being parsed.</param>
    public delegate void BlockParse(IniFile init);

    private const int MaxCharsPerLine = 0x04_04;

    // TODO: Implement actual parsing logic for different block types.
    private static readonly BlockParseStruct[] TypeTable = [new() { Token = "ArrayData", Parse = null! }];

    private readonly byte[] _lineBuffer = new byte[MaxCharsPerLine];

    private Xfer? _xfer;
    private MemoryStream? _file;
    private string _fileName = "None";
    private IniLoadType _loadType;
    private int _lineNumber = 1;

    // Index into the current _lineBuffer used by GetNextToken() to emulate strtok(NULL, seps)
    private int _tokenIndex;
    private bool _disposedValue;

    /// <summary>
    /// Gets the set of separator characters used in INI files.
    /// </summary>
    protected IReadOnlyList<char> Separators { get; } = "\n\r\t=".ToCharArray();

    /// <summary>
    /// Gets the set of separator characters including '%' used in INI files.
    /// </summary>
    protected IReadOnlyList<char> SeparatorsPercent { get; } = "\n\r\t=%".ToCharArray();

    /// <summary>
    /// Gets the set of separator characters including ':', used in INI files.
    /// </summary>
    protected IReadOnlyList<char> SeparatorsColor { get; } = "\n\r\t=:".ToCharArray();

    /// <summary>
    /// Gets the set of separator characters including '"', used in INI files.
    /// </summary>
    protected IReadOnlyList<char> SeparatorsQuote { get; } = "\"\n=".ToCharArray();

    /// <summary>
    /// Determines if the provided buffer contains a declaration of the specified block type and name.
    /// </summary>
    /// <param name="blockType">The type of the block to check for (e.g., "Object").</param>
    /// <param name="blockName">The name of the block to check for (e.g., "MyObject").</param>
    /// <param name="bufferToCheck">The buffer to check for the block declaration.</param>
    /// <returns><see langword="true"/> if the buffer contains the block declaration; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="blockType"/> or <paramref name="blockName"/> is null, empty, or whitespace.</exception>
    /// <remarks>
    /// This method checks the provided buffer for a specific block declaration in the INI file format.
    /// </remarks>
    public static bool IsDeclarationOfType([NotNull] string blockType, string blockName, Span<byte> bufferToCheck)
    {
        if (string.IsNullOrWhiteSpace(blockType))
        {
            throw new ArgumentException("Block type cannot be null or whitespace.", nameof(blockType));
        }

        if (string.IsNullOrEmpty(blockType))
        {
            throw new ArgumentException("Block type cannot be null or empty.", nameof(blockType));
        }

        if (string.IsNullOrWhiteSpace(blockName))
        {
            throw new ArgumentException("Block name cannot be null or whitespace.", nameof(blockName));
        }

        if (string.IsNullOrEmpty(blockName))
        {
            throw new ArgumentException("Block name cannot be null or empty.", nameof(blockName));
        }

        var bufferIndex = 0;

        // Skip leading whitespace
        while (bufferToCheck[bufferIndex] is (byte)' ')
        {
            bufferIndex++;
        }

        // Check that the buffer starts with the block type
        if (bufferToCheck.Slice(bufferIndex, blockType.Length).Length != blockType.Length)
        {
            return false;
        }

        bufferIndex += blockType.Length;
        if (bufferToCheck[bufferIndex++] is not (byte)' ')
        {
            // There must be a space after the block type
            return false;
        }

        // Skip whitespace between block type and block name
        while (bufferToCheck[bufferIndex] is (byte)' ')
        {
            bufferIndex++;
        }

        // Check that the buffer contains the block name
        if (bufferToCheck.Slice(bufferIndex, blockName.Length).Length != blockName.Length)
        {
            return false;
        }

        bufferIndex += blockName.Length;

        // Skip whitespace after block name
        while (bufferToCheck[bufferIndex] is (byte)' ')
        {
            bufferIndex++;
        }

        if (bufferToCheck[bufferIndex] is not (byte)'\0')
        {
            // There must be nothing else after the block name
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines if the provided buffer indicates the end of a block.
    /// </summary>
    /// <param name="bufferToCheck">The buffer to check for the end of block declaration.</param>
    /// <returns><see langword="true"/> if the buffer indicates the end of a block; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This method checks the provided buffer for a specific end of block declaration in the INI file format.
    /// </remarks>
    public static bool IsEndOfBlock(Span<byte> bufferToCheck)
    {
        var returnValue = true;
        if (bufferToCheck.Length == 0)
        {
            return false;
        }

        const string endString = "End";
        byte restoreChar;

        var bufferIndex = 0;

        // Skip leading whitespace
        while (bufferToCheck[bufferIndex] is (byte)' ')
        {
            bufferIndex++;
        }

        if (bufferToCheck.Slice(bufferIndex, endString.Length).Length > endString.Length)
        {
            restoreChar = bufferToCheck[bufferIndex + endString.Length];
            bufferToCheck[bufferIndex + endString.Length] = 0;

            if (
                LegacyEncodings
                    .Ansi.GetString(bufferToCheck.Slice(bufferIndex, endString.Length))
                    .Equals(endString, StringComparison.OrdinalIgnoreCase)
            )
            {
                returnValue = false;
            }

            bufferToCheck[bufferIndex + endString.Length] = restoreChar;
            bufferIndex += endString.Length;
        }
        else
        {
            returnValue = false;
        }

        while (bufferToCheck[bufferIndex] != 0 && returnValue)
        {
            returnValue = bufferToCheck[bufferIndex] is (byte)' ';
            bufferIndex++;
        }

        return returnValue;
    }

    /// <summary>
    /// Loads all INI files in the specified directory.
    /// </summary>
    /// <param name="dirName">The directory to load INI files from.</param>
    /// <param name="subDirs">Whether to include subdirectories.</param>
    /// <param name="loadType">The type of loading behavior.</param>
    /// <param name="xfer">An optional Xfer object for data transfer.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="dirName"/> is null, empty, or whitespace.</exception>
    /// <remarks>
    /// This method loads all INI files in the specified directory, optionally including subdirectories.
    /// </remarks>
    public void LoadDirectory([NotNull] string dirName, bool subDirs, IniLoadType loadType, Xfer? xfer = null)
    {
        if (string.IsNullOrWhiteSpace(dirName))
        {
            throw new ArgumentException("Directory name cannot be null or whitespace.", nameof(dirName));
        }

        if (string.IsNullOrEmpty(dirName))
        {
            throw new ArgumentException("Directory name cannot be null or empty.", nameof(dirName));
        }

        var filesInDir = Directory.GetFiles(
            dirName,
            "*.ini",
            subDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
        );

        foreach (var file in filesInDir)
        {
            // Load the files that are NOT in a subdirectory first.
            if (Path.GetDirectoryName(file)?.Equals(dirName, StringComparison.OrdinalIgnoreCase) == true)
            {
                Load(file, loadType, xfer);
            }
        }

        if (!subDirs)
        {
            return;
        }

        foreach (var file in filesInDir)
        {
            // Load the files that ARE in a subdirectory next.
            if (Path.GetDirectoryName(file)?.Equals(dirName, StringComparison.OrdinalIgnoreCase) == false)
            {
                Load(file, loadType, xfer);
            }
        }
    }

    /// <summary>
    /// Loads the specified INI file.
    /// </summary>
    /// <param name="name">The name of the INI file to load.</param>
    /// <param name="loadType">The type of loading behavior.</param>
    /// <param name="xfer">An optional Xfer object for data transfer.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the Ini instance has been disposed.</exception>
    /// <exception cref="ArgumentException">Thrown if the file name is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a file is already open or if the file stream is not initialized.</exception>
    /// <exception cref="EndOfStreamException">Thrown if the file stream is at the end of the file before reading.</exception>
    /// <exception cref="InvalidDataException">Thrown if the INI file contains invalid data.</exception>
    public void Load(string name, IniLoadType loadType, Xfer? xfer = null)
    {
        ObjectDisposedException.ThrowIf(_disposedValue, this);

        if (!IsValidIniFilename(name))
        {
            throw new ArgumentException($"Invalid INI file name '{name}'.", nameof(name));
        }

        if (_file is null)
        {
            throw new InvalidOperationException("File stream is not initialized.");
        }

        if (_file.Position == _file.Length)
        {
            throw new EndOfStreamException("File stream is at the end of the file before reading.");
        }

        try
        {
            _xfer = xfer;
            PrepareFile(name, loadType);

            while (_file.Position < _file.Length)
            {
                ReadLine();

                var currentLine = LegacyEncodings.Ansi.GetString(_lineBuffer).TrimEnd([.. Separators]);
                var tokens = currentLine.Split([.. Separators], StringSplitOptions.RemoveEmptyEntries);
                if (tokens is not null)
                {
                    BlockParse? parse = FindBlockParse(tokens[0]);
                    if (parse is not null)
                    {
                        try
                        {
                            parse(this);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException(
                                $"Error parsing block starting at line {_lineNumber} in INI file '{_fileName}'.",
                                ex
                            );
                        }
                    }
                    else
                    {
                        throw new InvalidDataException(
                            $"Unknown block type '{tokens[0]}' at line {_lineNumber} in INI file '{_fileName}'."
                        );
                    }
                }
            }
        }
        finally
        {
            UnprepareFile();
        }
    }

    /// <summary>
    /// Returns the next token from the current line buffer using the provided separators.
    /// Subsequent calls continue from the last returned token (emulates strtok(NULL, seps)).
    /// </summary>
    /// <param name="separators">Optional separators to use. If null, the default <see cref="Separators"/> are used.</param>
    /// <returns>The next token as a string.</returns>
    /// <exception cref="InvalidDataException">Thrown when no more tokens are available or an unexpected end is encountered.</exception>
    /// <remarks>
    /// This method uses the internal line buffer and token index to return tokens sequentially. It is equivalent to calling strtok(NULL, seps) in C.
    /// </remarks>
    public string GetNextToken(IReadOnlyList<char>? separators = null)
    {
        IReadOnlyList<char> seps = separators ?? Separators;

        // Start scanning from the last position.
        var idx = int.Max(_tokenIndex, 0);
        if (idx >= _lineBuffer.Length)
        {
            throw new InvalidDataException("No more tokens available in the current line.");
        }

        // Skip leading separators
        while (_lineBuffer[idx] != 0 && IsSeparator(_lineBuffer[idx], seps))
        {
            idx++;
            if (idx >= _lineBuffer.Length)
            {
                throw new InvalidDataException("No more tokens available in the current line.");
            }
        }

        if (_lineBuffer[idx] == 0)
        {
            throw new InvalidDataException("Unexpected end of tokens.");
        }

        var start = idx;
        while (_lineBuffer[idx] != 0 && !IsSeparator(_lineBuffer[idx], seps))
        {
            idx++;
            if (idx >= _lineBuffer.Length)
            {
                break;
            }
        }

        var length = idx - start;
        var token = LegacyEncodings.Ansi.GetString(_lineBuffer.AsSpan(start, length));

        _tokenIndex = idx;
        return token;

        // Helper to check if a byte is one of the separator characters.
        static bool IsSeparator(byte b, IReadOnlyList<char> separators)
        {
            foreach (var c in separators)
            {
                if ((byte)c == b)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Validates the INI file name.
    /// </summary>
    /// <param name="fileName">The file name to validate.</param>
    /// <returns><see langword="true"/> if the file name is valid; otherwise, <see langword="false"/>.</returns>
    /// <remarks> This method checks if the file name is not null, empty, or whitespace and ends with ".ini".</remarks>
    protected static bool IsValidIniFilename([NotNull] string fileName) =>
        !string.IsNullOrWhiteSpace(fileName)
        && !string.IsNullOrEmpty(fileName)
        && fileName.EndsWith(".ini", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Prepares the INI file for loading.
    /// </summary>
    /// <param name="fileName">The name of the INI file to prepare.</param>
    /// <param name="loadType">The type of loading behavior.</param>
    /// <exception cref="ObjectDisposedException">Thrown if the Ini instance has been disposed.</exception>
    /// <exception cref="ArgumentException">Thrown if the file name is null, empty, or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a file is already open.</exception>
    /// <remarks>This method opens the file and initializes related fields.</remarks>
    /// <seealso cref="UnprepareFile()"/>
    [MemberNotNull(nameof(_file))]
    protected void PrepareFile([NotNull] string fileName, IniLoadType loadType)
    {
        ObjectDisposedException.ThrowIf(_disposedValue, this);

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        if (_file is not null)
        {
            throw new InvalidOperationException(
                $"File '{fileName}' is already open. Close it before opening it again."
            );
        }

        _fileName = fileName;
        _file = new MemoryStream(File.ReadAllBytes(fileName));
        _loadType = loadType;
    }

    /// <summary>
    /// Cleans up after loading the INI file.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the Ini instance has been disposed.</exception>
    /// <remarks>This method disposes of the file stream and resets related fields.</remarks>
    /// <seealso cref="PrepareFile(string, IniLoadType)"/>
    protected void UnprepareFile()
    {
        ObjectDisposedException.ThrowIf(_disposedValue, this);

        _file?.Dispose();
        _file = null;
        _fileName = "None";
        _loadType = IniLoadType.Invalid;
        _lineNumber = 1;
        _xfer = null;
    }

    /// <summary>
    /// Reads a single line from the INI file into the line buffer.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the file stream is not initialized.</exception>
    /// <exception cref="InvalidDataException">Thrown if a line exceeds the maximum length or contains tabs.</exception>
    protected void ReadLine()
    {
        var isComment = false;
        if (_file is null)
        {
            throw new InvalidOperationException("File stream is not initialized.");
        }

        if (_file.Position == _file.Length)
        {
            _lineBuffer[0] = (byte)'\0';
        }
        else
        {
            var done = false;
            while (!done)
            {
                var eof = _file.ReadByte() == 0;
                if (eof)
                {
                    done = true;
                    _lineBuffer[_file.Position] = (byte)'\0';
                }

                if (_lineBuffer[_file.Position] == '\n')
                {
                    done = true;
                }

                if (_lineBuffer[_file.Position] == '\t')
                {
                    throw new InvalidDataException(
                        $"Tabs are not allowed in INI files ({_fileName}). Please check your editor settings. Line Number {_lineNumber}"
                    );
                }

                // Replace control characters with spaces
                if (_lineBuffer[_file.Position] is < 32)
                {
                    _lineBuffer[_file.Position] = (byte)' ';
                }

                if (_lineBuffer[_file.Position] == ';')
                {
                    isComment = true;
                }

                if (isComment)
                {
                    _lineBuffer[_file.Position] = (byte)'\0';
                }

                if (done && _file.Position + 1 < MaxCharsPerLine)
                {
                    _lineBuffer[_file.Position + 1] = (byte)'\0';
                }

                // Increment position and check for overflow
                if (_file.Position++ == MaxCharsPerLine)
                {
                    done = true;
                }
            }

            _lineNumber++;

            if (_file.Position == MaxCharsPerLine)
            {
                throw new InvalidDataException(
                    $"Line exceeds maximum length of {MaxCharsPerLine} characters in INI file '{_fileName}'. Line Number {_lineNumber}"
                );
            }

            _xfer?.User(_lineBuffer);

            // Reset token index so GetNextToken starts at the beginning of the new line
            _tokenIndex = 0;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            _file?.Dispose();

            _file = null;
            _xfer = null;

            _fileName = "None";
            _loadType = IniLoadType.Invalid;
            _lineNumber = 1;
        }

        _disposedValue = true;
    }

    /// <summary>
    /// Finds the block parse delegate for the specified token.
    /// </summary>
    /// <param name="token">The token to find the block parse for.</param>
    /// <returns>The block parse delegate if found; otherwise, <see langword="null"/>.</returns>
    /// <remarks>This method searches the type table for a matching token and returns the associated parse delegate.</remarks>
    private static BlockParse? FindBlockParse([NotNull] string token)
    {
        foreach (BlockParseStruct parse in TypeTable)
        {
            if (parse.Token.Equals(token, StringComparison.OrdinalIgnoreCase))
            {
                return parse.Parse;
            }
        }

        return null;
    }

    private struct BlockParseStruct
    {
        public string Token;
        public BlockParse Parse;
    }
}
