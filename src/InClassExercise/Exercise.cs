// <copyright file="Exercise.cs" company="Garbagemen">
// Copyright (c) Garbagemen. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace InClassExercise;

/// <summary>
/// This exercise demonstrates a subtle memory leak pattern.
/// Identify why objects are not being garbage collected and implement a fix.
/// </summary>
public class EventPublisher
{
    // TODO: This event may be causing a memory leak.  Why?
    public event EventHandler<string>? OnEvent;

    public void RaiseEvent(string message)
    {
        OnEvent?.Invoke(this, message);
    }
}

/// <summary>
/// This class subscribes to events from EventPublisher.  Identify the leak and fix it.
/// </summary>
public class EventSubscriber
{
    private readonly string name;
    private readonly EventPublisher publisher;

    public EventSubscriber(string name, EventPublisher publisher)
    {
        this.name = name;
        this.publisher = publisher;

        // TODO: Is this subscription causing a problem?
        this.publisher.OnEvent += HandleEvent;
    }

    private void HandleEvent(object? sender, string message)
    {
        Console.WriteLine($"[{this.name}] Received: {message}");
    }

    // TODO: What's missing here that would prevent garbage collection?
    // TODO: Implement a fix to allow this object to be collected.
}

/// <summary>
/// Demo class for In-Class Exercise - Event Leak Exercise.
/// </summary>
public class InClassExercise
{
    /// <summary>
    /// Runs the demo.
    /// </summary>
    public static void Run()
    {
        var publisher = new EventPublisher();
        var subscribers = new List<EventSubscriber>();

        Console.WriteLine("Creating subscribers...");

        // TODO: What happens to these subscriber objects?
        for (int i = 0; i < 1000; i++)
        {
            var subscriber = new EventSubscriber($"Subscriber [{i}]", publisher);
            subscribers.Add(subscriber);
        }

        Console.WriteLine($"Created {subscribers.Count} subscribers.");
        Console.WriteLine("Clearing list of subscribers...");

        // TODO: After clearing, can the subscriber objects be garbage collected?
        subscribers.Clear();

        // Force GC for demonstration (not recommended in production):
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Console.WriteLine("Subscribers list cleared.  Check memory; are objects collected?");
        Console.WriteLine("Press any key to raise an event...");
        Console.ReadKey();

        publisher.RaiseEvent("Test message");

        // TODO: Use a memory profiler or debugger to verify if objects are collected.
        // TODO: Identify the root cause and implement a fix.
    }
}