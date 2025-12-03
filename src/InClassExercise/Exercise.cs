// <copyright file="Exercise.cs" company="Garbagemen">
// Copyright (c) Garbagemen. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace InClassExercise;

/// <summary>
/// This exercise demonstrates a subtle memory leak pattern.
/// Identify why objects are not being garbage collected and implement a fix.
/// Event publisher that can raise events to subscribers.
/// </summary>
public class EventPublisher
{
    /// <summary>
    /// Event that is raised when RaiseEvent is called.
    /// TODO: This event may be causing a memory leak.  Why?
    /// </summary>
    public event EventHandler<string>? OnEvent;

    /// <summary>
    /// Raises the OnEvent event with the specified message.
    /// </summary>
    /// <param name="message">The message to send to subscribers.</param>
    public void RaiseEvent(string message)
    {
        this.OnEvent?.Invoke(this, message);
    }
}

/// <summary>
/// This class subscribes to events from EventPublisher.  Identify the leak and fix it.
/// </summary>
public class EventSubscriber
{
    private readonly string name;
    private readonly EventPublisher publisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSubscriber"/> class.
    /// </summary>
    /// <param name="name">The name of the subscriber.</param>
    /// <param name="publisher">The event publisher to subscribe to.</param>
    public EventSubscriber(string name, EventPublisher publisher)
    {
        this.name = name;
        this.publisher = publisher;

        // TODO: Is this subscription causing a problem?
        this.publisher.OnEvent += this.HandleEvent;
    }

    private void HandleEvent(object? sender, string message)
    {
        Console.WriteLine($"[{this.name}] Received: {message}");
    }

    // TODO: What's missing here that would prevent garbage collection?
    // TODO: Implement a fix to allow this object to be collected.
}

/// <summary>
/// Solution version of EventSubscriber that implements IDisposable to fix the memory leak.
/// </summary>
public class EventSubscriberSolution : IDisposable
{
    private readonly string name;
    private readonly EventPublisher publisher;
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSubscriberSolution"/> class.
    /// </summary>
    /// <param name="name">The name of the subscriber.</param>
    /// <param name="publisher">The event publisher to subscribe to.</param>
    public EventSubscriberSolution(string name, EventPublisher publisher)
    {
        this.name = name;
        this.publisher = publisher;
        this.publisher.OnEvent += this.HandleEvent;
    }

    /// <summary>
    /// Disposes the subscriber and unsubscribes from the event.
    /// </summary>
    public void Dispose()
    {
        if (!this.disposed)
        {
            // Unsubscribe from the event to break the reference chain.
            this.publisher.OnEvent -= this.HandleEvent;
            this.disposed = true;
        }
    }

    private void HandleEvent(object? sender, string message)
    {
        if (!this.disposed)
        {
            Console.WriteLine($"[{this.name}] Received: {message}");
        }
    }
}

/// <summary>
/// Demo class for In-Class Exercise - Event Leak Exercise.
/// </summary>
public class InClassExercise
{
    /// <summary>
    /// Runs the demo with a menu to choose between exercise and solution versions.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("IN-CLASS EXERCISE");
        Console.WriteLine();
        Console.WriteLine("1  |  Run Exercise (with memory leak)");
        Console.WriteLine("2  |  Run Solution (fixed)");
        Console.WriteLine("0  |  Exit");
        Console.Write("SELECT:  ");

        string? choice = Console.ReadLine();
        Console.WriteLine();

        if (choice == "1")
        {
            RunExercise();
        }
        else if (choice == "2")
        {
            RunSolution();
        }
        else if (choice == "0")
        {
            Console.WriteLine("Thanks!  Have a good one!");
            return;
        }
        else
        {
            Console.WriteLine("INVALID: PLEASE TRY AGAIN.");
        }
    }

    /// <summary>
    /// Runs the exercise version that demonstrates the memory leak.
    /// </summary>
    private static void RunExercise()
    {
        var publisher = new EventPublisher();
        var subscribers = new List<EventSubscriber>();

        Console.WriteLine("EXERCISE VERSION (WITH MEMORY LEAK)");
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

    /// <summary>
    /// Runs the solution version that properly disposes subscribers.
    /// </summary>
    private static void RunSolution()
    {
        var publisher = new EventPublisher();
        var subscribers = new List<EventSubscriberSolution>();

        Console.WriteLine("SOLUTION VERSION (FIXED)");
        Console.WriteLine("Creating subscribers...");

        for (int i = 0; i < 1000; i++)
        {
            var subscriber = new EventSubscriberSolution($"Subscriber [{i}]", publisher);
            subscribers.Add(subscriber);
        }

        Console.WriteLine($"Created {subscribers.Count} subscribers.");
        Console.WriteLine("Disposing subscribers...");

        // Dispose all subscribers before clearing.
        foreach (var subscriber in subscribers)
        {
            subscriber.Dispose();
        }

        subscribers.Clear();

        // Force GC for demonstration.
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Console.WriteLine("Subscribers disposed and list cleared.  Objects should now be collectable.");
        Console.WriteLine("Press any key to raise an event...");
        Console.ReadKey();

        publisher.RaiseEvent("Test message");

        // With the fix, subscribers should be collected and this should not print anything
        // or print only if there are other active subscribers.
    }
}