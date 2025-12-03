// <copyright file="LeakyListener.cs" company="Alex">
// Copyright (c) Alex. All rights reserved.
// </copyright>

using System;

namespace AlexDemo;

/// <summary>
/// Listener that subscribes to events but never unsubscribes, causing memory leaks.
/// </summary>
class LeakyListener
{
    private byte[] buffer = new byte[1024 * 1024];  // 1 MB.
    private int id;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeakyListener"/> class.
    /// </summary>
    /// <param name="id">The listener ID.</param>
    public LeakyListener(int id)
    {
        this.id = id;
        GlobalTimer.Tick += OnTick;
    }

    private void OnTick(object? sender, EventArgs e) { }
}