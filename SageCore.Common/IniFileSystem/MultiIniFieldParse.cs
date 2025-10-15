// -----------------------------------------------------------------------
// <copyright file="MultiIniFieldParse.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Errors;

namespace SageCore.Common.IniFileSystem;

/// <summary>
/// Handles multiple field parsers for INI file processing.
/// </summary>
/// <typeparam name="TInstance">The type of the instance being processed.</typeparam>
/// <typeparam name="TStore">The type of the store used during processing.</typeparam>
public class MultiIniFieldParse<TInstance, TStore>
    where TStore : class
{
    private const int MaxMultiFields = 16;

    private readonly FieldParse<TInstance, TStore>[] _fieldParse = new FieldParse<TInstance, TStore>[MaxMultiFields];
    private readonly uint[] _extraOffset = new uint[MaxMultiFields];

    /// <summary>
    /// Gets the count of field parsers added.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Adds a new field parser to the collection.
    /// </summary>
    /// <param name="fieldParse">The field parser to add.</param>
    /// <param name="extraOffset">The extra offset for the field parser.</param>
    public void Add(FieldParse<TInstance, TStore> fieldParse, uint extraOffset = 0)
    {
        if (Count >= MaxMultiFields)
        {
            throw new SageBugException($"Too many multi-fields in {nameof(Ini)}.{nameof(Ini.InitFromMultiProcess)}");
        }

        if (Count < MaxMultiFields)
        {
            _fieldParse[Count] = fieldParse;
            _extraOffset[Count] = extraOffset;
            Count++;
        }
    }

    /// <summary>
    /// Gets the field parser at the specified index.
    /// </summary>
    /// <param name="index">The index of the field parser to retrieve.</param>
    /// <returns>The field parser at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public FieldParse<TInstance, TStore> GetFieldParse(int index) =>
        index < 0 || index >= Count
            ? throw new ArgumentOutOfRangeException(nameof(index), index, "Index is out of range.")
            : _fieldParse[index];

    /// <summary>
    /// Gets the extra offset for the field parser at the specified index.
    /// </summary>
    /// <param name="index">The index of the field parser to retrieve the extra offset for.</param>
    /// <returns>The extra offset for the field parser at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is out of range.</exception>
    public uint GetExtraOffset(int index) =>
        index < 0 || index >= Count
            ? throw new ArgumentOutOfRangeException(nameof(index), index, "Index is out of range.")
            : _extraOffset[index];
}
