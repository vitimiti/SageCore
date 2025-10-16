// -----------------------------------------------------------------------
// <copyright file="ISnapshot.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Common.Data;

/// <summary>
/// Defines methods for snapshotting and restoring object state.
/// </summary>
public interface ISnapshot
{
    /// <summary>
    /// Calculates and updates the CRC for the object state using the provided transfer mechanism.
    /// </summary>
    /// <param name="xfer">The transfer mechanism to use for CRC calculation.</param>
    /// <remarks>This method is intended to be called before saving the object state to ensure data integrity.</remarks>
    void Crc(Xfer xfer);

    /// <summary>
    /// Transfers the object state using the provided transfer mechanism.
    /// </summary>
    /// <param name="xfer">The transfer mechanism to use for state transfer.</param>
    void Xfer(Xfer xfer);

    /// <summary>
    /// Performs post-processing after loading the object state.
    /// </summary>
    void LoadPostProcess();
}
