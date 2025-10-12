// -----------------------------------------------------------------------
// <copyright file="ThingFactory.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace SageCore.Subsystems;

internal sealed class ThingFactory : SubsystemBase
{
    private static ThingFactory? _instance;

    private ThingFactory() { }

    public static ThingFactory Instance => _instance ??= new ThingFactory();

    public override void Initialize() => throw new NotImplementedException();

    public override void Reset() => throw new NotImplementedException();

    protected override void Draw() => throw new NotImplementedException();

    protected override void Update() => throw new NotImplementedException();
}
