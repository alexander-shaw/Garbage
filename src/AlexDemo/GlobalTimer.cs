// <copyright file="GlobalTimer.cs" company="Alex">
// Copyright (c) Alex. All rights reserved.
// </copyright>

using System;

namespace AlexDemo;

/// <summary>
/// Static timer class that publishes tick events.
/// </summary>
static class GlobalTimer
{
    /// <summary>
    /// Event that fires on tick.
    /// </summary>
    public static event EventHandler? Tick;

    /// <summary>
    /// Raises the Tick event.
    /// </summary>
    public static void RaiseTick()
    {
        Tick?.Invoke(null, EventArgs.Empty);
    }
}