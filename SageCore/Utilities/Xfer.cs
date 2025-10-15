// -----------------------------------------------------------------------
// <copyright file="Xfer.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Utilities;

internal abstract class Xfer
{
    public uint Version(uint version, uint currentVersion)
    {
        version = Implementation(version);
        return version > currentVersion
            ? throw new InvalidDataException(
                $"{nameof(Xfer)}.{nameof(Version)}: Unknown version '{version}' should be no higher than '{currentVersion}'"
            )
            : version;
    }

    public bool Bool(bool data) => Implementation(data);

    public float Real(float data) => Implementation(data);

    public abstract T User<T>(T data);

    protected abstract T Implementation<T>(T data);
}
