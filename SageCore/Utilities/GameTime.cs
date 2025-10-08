// -----------------------------------------------------------------------
// <copyright file="GameTime.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace SageCore.Utilities;

/// <summary>
/// Provides timing values for the game loop, including total elapsed time and delta time between frames.
/// </summary>
[DebuggerDisplay("{TotalTime} (+{DeltaTime}) @ {TickRatio} ticks/stopwatch tick")]
internal sealed class GameTime
{
    private readonly Stopwatch _stopwatch = new();

    private long _startTime;
    private long _lastTime;
    private long _currentTime;

    /// <summary>
    /// Gets the ratio of TimeSpan ticks to Stopwatch ticks.
    /// </summary>
    public static double TickRatio => (double)TimeSpan.TicksPerSecond / Stopwatch.Frequency;

    /// <summary>
    /// Gets the total elapsed time since the game started.
    /// </summary>
    public TimeSpan TotalTime { get; private set; }

    /// <summary>
    /// Gets the time elapsed since the last update.
    /// </summary>
    public TimeSpan DeltaTime { get; private set; }

    /// <summary>
    /// Gets the total elapsed time in seconds since the game started.
    /// </summary>
    public float TotalTimeSeconds { get; private set; }

    /// <summary>
    /// Gets the time elapsed in seconds since the last update.
    /// </summary>
    public float DeltaTimeSeconds { get; private set; }

    /// <summary>
    /// Gets the maximum allowed delta time to prevent large jumps in time (e.g., when debugging or if the game is paused).
    /// </summary>
    private static TimeSpan MaxDeltaTime => TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Initializes a new instance of the <see cref="GameTime"/> class and starts the stopwatch.
    /// </summary>
    public GameTime()
    {
        _stopwatch.Start();
        _startTime = _stopwatch.ElapsedTicks;
        _lastTime = _startTime;
        _currentTime = _startTime;
    }

    /// <summary>
    /// Updates the timing values. Should be called once per frame.
    /// </summary>
    public void Update()
    {
        _lastTime = _currentTime;
        _currentTime = _stopwatch.ElapsedTicks;

        // Cache calculated values
        TotalTime = new TimeSpan((long)((_currentTime - _startTime) * TickRatio));

        TimeSpan delta = new((long)((_currentTime - _lastTime) * TickRatio));
        DeltaTime = delta > MaxDeltaTime ? MaxDeltaTime : delta;

        TotalTimeSeconds = (float)TotalTime.TotalSeconds;
        DeltaTimeSeconds = (float)DeltaTime.TotalSeconds;
    }

    /// <summary>
    /// Resets the game time to the current time, effectively restarting the timing.
    /// </summary>
    public void Reset()
    {
        _startTime = _stopwatch.ElapsedTicks;
        _lastTime = _startTime;
        _currentTime = _startTime;
    }
}
