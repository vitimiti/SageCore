// -----------------------------------------------------------------------
// <copyright file="Pixels.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SageCore.NativeMethods.Sdl3;

internal static partial class Sdl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FColor
    {
        public float R;
        public float G;
        public float B;
        public float A;
    }
}
