// -----------------------------------------------------------------------
// <copyright file="SdlOwnedUtf8StringMarshaller.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;

namespace SageCore.Platform.Sdl3.NativeMethods.CustomMarshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ManagedToUnmanagedOut))]
internal static unsafe class SdlOwnedUtf8StringMarshaller
{
    public ref struct ManagedToUnmanagedOut
    {
        private byte* _unmanaged;
        private string? _managed;

        public void FromUnmanaged(byte* unmanaged)
        {
            if (unmanaged is null)
            {
                _unmanaged = null;
                _managed = null;
            }

            _unmanaged = Sdl.StrDup(unmanaged);
            _managed = Utf8StringMarshaller.ConvertToManaged(unmanaged);
        }

        public readonly string? ToManaged() => _managed;

        public readonly void Free() => Sdl.Free(_unmanaged);
    }
}
