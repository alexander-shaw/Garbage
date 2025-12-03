// <copyright file="SafeListener.cs" company="Alex">
// Copyright (c) Alex. All rights reserved.
// </copyright>

using System;

namespace AlexDemo;

/// <summary>
/// Listener that properly unsubscribes from events using IDisposable.
/// </summary>
class SafeListener : IDisposable
{
    private byte[] buffer = new byte[1024 * 1024];  // 1 MB.
    private int id;
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeListener"/> class.
    /// </summary>
    /// <param name="id">The listener ID.</param>
    public SafeListener(int id)
    {
        this.id = id;
        GlobalTimer.Tick += OnTick;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (disposed) return;
    }

    /// <summary>
    /// Disposes the listener and unsubscribes from the event.
    /// </summary>
    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        GlobalTimer.Tick -= OnTick;  // Unsubscribe!
    }
}