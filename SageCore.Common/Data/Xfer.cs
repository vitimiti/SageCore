// -----------------------------------------------------------------------
// <copyright file="Xfer.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Common.Data.Exceptions;

namespace SageCore.Common.Data;

public abstract class Xfer
{
    protected Xfer() { }

    public XferOptions Options { get; set; } = XferOptions.None;

    public XferMode Mode { get; set; } = XferMode.Invalid;

    public string? Identifier { get; private set; }

    public abstract void Open(string identifier);

    public abstract void Close();

    public abstract int BeginBlock();

    public abstract void EndBlock();

    public abstract void Skip(int dataSize);

    public abstract void Snapshot(ISnapshot snapshot);

    public virtual uint Version(uint versionData, uint currentVerion)
    {
        versionData = Implementation(versionData);
        return versionData > currentVerion
            ? throw new SageXferInvalidVersionException(
                $"Data version {versionData} is newer than supported version {currentVerion}."
            )
            : versionData;
    }

    public virtual sbyte SByte(sbyte data) => Implementation(data);

    public virtual byte Byte(byte data) => Implementation(data);

    public virtual bool Bool(bool data) => Implementation(data);

    public virtual int Int(int data) => Implementation(data);

    public virtual long Int64(long data) => Implementation(data);

    public virtual uint UInt(uint data) => Implementation(data);

    public virtual ulong UInt64(ulong data) => Implementation(data);

    public virtual short Short(short data) => Implementation(data);

    public virtual ushort UShort(ushort data) => Implementation(data);

    public virtual float Real(float data) => Implementation(data);

    public virtual string MarkerLabel(string data) => string.Empty;

    public virtual string String(string data) => Implementation(data);

    protected abstract T Implementation<T>(T data);
}
