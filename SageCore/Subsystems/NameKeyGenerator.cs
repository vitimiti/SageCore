// -----------------------------------------------------------------------
// <copyright file="NameKeyGenerator.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using SageCore.Enums;
using SageCore.Utilities;

namespace SageCore.Subsystems;

internal sealed class NameKeyGenerator : SubsystemBase
{
    // A prime number that isn't a power of two.
    private const int SocketCount = 0xAF_CF;

    private readonly Bucket?[] _sockets = new Bucket[SocketCount];

    private uint _nextId;

    private static NameKeyGenerator? _instance;

    private NameKeyGenerator() { }

    public static NameKeyGenerator Instance => _instance ??= new NameKeyGenerator();

    public override void Initialize()
    {
        if (_nextId != (uint)NameKeyType.Invalid)
        {
            throw new InvalidOperationException($"{nameof(NameKeyGenerator)} already initialized.");
        }

        FreeSockets();
        _nextId = 1;
    }

    public override void Reset()
    {
        FreeSockets();
        _nextId = 1;
    }

    public override void Draw() { }

    public override void Update() { }

    public NameKeyType NameToKey(string name)
    {
        var hash = CalculateHashForString(name) % SocketCount;
        Bucket? bucket;
        for (bucket = _sockets[hash]; bucket is not null; bucket = bucket.NextInSocket)
        {
            if (name.Equals(bucket.Name, StringComparison.Ordinal))
            {
                return bucket.Key;
            }
        }

        bucket = new Bucket
        {
            Key = (NameKeyType)_nextId++,
            Name = name,
            NextInSocket = _sockets[hash],
        };

        _sockets[hash] = bucket;
        NameKeyType result = bucket.Key;

#if DEBUG
        const int maxThreshold = 3;
        var overThresholdCount = 0;
        for (var i = 0; i < SocketCount; i++)
        {
            var inThisSocketCount = 0;
            for (bucket = _sockets[i]; bucket is not null; bucket = bucket.NextInSocket)
            {
                inThisSocketCount++;
            }

            if (inThisSocketCount > maxThreshold)
            {
                overThresholdCount++;
            }
        }

        if (overThresholdCount > SocketCount / 20)
        {
            throw new InvalidOperationException(
                $"We might need to increase the number of bucket-sockets for {nameof(NameKeyGenerator)} ({nameof(overThresholdCount)} {overThresholdCount} = {overThresholdCount / (float)(SocketCount / 20F):F2})\n\tThis should never happen and is a consequence of custom mods that are going overboard with the number of bucket-sockets or due to the new engine needs."
            );
        }
#endif

        return result;
    }

    public NameKeyType NameToLowercaseKey(string name)
    {
        var hash = CalculateHashForLowercaseString(name) % SocketCount;
        Bucket? bucket;
        for (bucket = _sockets[hash]; bucket is not null; bucket = bucket.NextInSocket)
        {
            if (string.Equals(name, bucket.Name, StringComparison.OrdinalIgnoreCase))
            {
                return bucket.Key;
            }
        }

        bucket = new Bucket
        {
            Key = (NameKeyType)_nextId++,
            Name = name,
            NextInSocket = _sockets[hash],
        };

        _sockets[hash] = bucket;

        NameKeyType result = bucket.Key;

#if DEBUG
        const int maxThreshold = 3;
        var overThresholdCount = 0;
        for (var i = 0; i < SocketCount; i++)
        {
            var inThisSocketCount = 0;
            for (bucket = _sockets[i]; bucket is not null; bucket = bucket.NextInSocket)
            {
                inThisSocketCount++;
            }

            if (inThisSocketCount > maxThreshold)
            {
                overThresholdCount++;
            }
        }

        if (overThresholdCount > SocketCount / 20)
        {
            throw new InvalidOperationException(
                $"We might need to increase the number of bucket-sockets for {nameof(NameKeyGenerator)} ({nameof(overThresholdCount)} {overThresholdCount} = {overThresholdCount / (float)(SocketCount / 20F):F2})\n\tThis should never happen and is a consequence of custom mods that are going overboard with the number of bucket-sockets or due to the new engine needs."
            );
        }
#endif

        return result;
    }

    private static uint CalculateHashForString(string str)
    {
        var result = 0U;
        foreach (var c in str)
        {
            result = (result << 5) + result + c;
        }

        return result;
    }

    private static uint CalculateHashForLowercaseString(string str)
    {
        var result = 0U;
        for (var i = 0; i < str.Length; i++)
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            result = (result << 5) + result + str.ToLowerInvariant()[i];
#pragma warning restore CA1308 // Normalize strings to uppercase
        }

        return result;
    }

    private void FreeSockets()
    {
        for (var i = 0; i < SocketCount; i++)
        {
            Bucket? next;
            for (Bucket? bucket = _sockets[i]; bucket is not null; bucket = next)
            {
                next = bucket.NextInSocket;
                bucket.Reset();
            }

            _sockets[i] = null;
        }
    }
}
