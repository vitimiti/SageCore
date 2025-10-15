// -----------------------------------------------------------------------
// <copyright file="ImageCollection.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using SageCore.FileSystem.Ini;
using SageCore.Utilities;

namespace SageCore.Subsystems;

internal class ImageCollection : SubsystemBase, IEnumerable<Image>
{
    private bool _disposedValue;

    protected Dictionary<uint, Image> ImageMap { get; } = [];

    public void Load(int textureSize)
    {
        IniFile ini = new();
    }

    public Image? FindImageByName(string name)
    {
        ObjectDisposedException.ThrowIf(_disposedValue, this);
        return ImageMap.TryGetValue((uint)NameKeyGenerator.Instance.NameToLowercaseKey(name), out Image? image)
            ? image
            : null;
    }

    public void AddImage([NotNull] Image image)
    {
        ObjectDisposedException.ThrowIf(_disposedValue, this);
        ImageMap[(uint)NameKeyGenerator.Instance.NameToLowercaseKey(image.Name)] = image;
    }

    public override void Initialize() { }

    public override void Reset() { }

    public override void Draw() { }

    public override void Update() { }

    public IEnumerator<Image> GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(_disposedValue, this);
        return ImageMap.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected override void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            foreach (Image image in ImageMap.Values)
            {
                image.Reset();
            }

            ImageMap.Clear();
        }

        base.Dispose(disposing);
        _disposedValue = true;
    }
}
