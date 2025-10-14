// -----------------------------------------------------------------------
// <copyright file="ISnapshot.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using SageCore.Utilities;

namespace SageCore.Interfaces;

internal interface ISnapshot
{
    void Crc(Xfer xfer);

    void Xfer(Xfer xfer);

    void LoadPostProcess();
}
