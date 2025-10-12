// -----------------------------------------------------------------------
// <copyright file="FileSubsystem.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Subsystems;

/// <summary>
/// Provides file system related utilities.
/// </summary>
public sealed class FileSubsystem : SubsystemBase
{
    /// <summary>
    /// Gets a case-insensitive sorted set of filenames.
    /// </summary>
    public static SortedSet<string> FilenameList { get; } = new(StringComparer.OrdinalIgnoreCase);

    public override void Initialize() => throw new NotImplementedException();

    public override void Reset() => throw new NotImplementedException();

    protected override void Draw() => throw new NotImplementedException();

    protected override void Update() => throw new NotImplementedException();
}
