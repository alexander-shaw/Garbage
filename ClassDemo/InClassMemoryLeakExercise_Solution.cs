using System;
using System.Collections.Generic;

namespace InClassMemoryLeakExercise;

/// <summary>
/// SOLUTION:
/// The memory leak occurs because EventPublisher holds strong references to all
/// subscribers through the event delegate chain.  When subscribers are no longer needed,
/// they cannot be garbage collected because the publisher still references them.
/// 
/// STEP 1:
/// Identify the long-lived publisher holding onto subscribers.
/// EventPublisher is long-lived and has an event with subscribers attached.
/// Each subscriber is kept alive by the event delegate.
/// 
/// STEP 2:
/// Implement IDisposable and unsubscribe in Dispose().
/// EventSubscriber should implement IDisposable.
/// In Dispose(), unsubscribe from the event: this.publisher.OnEvent -= HandleEvent;
/// 
/// STEP 3:
/// Ensure callers dispose instances (or use "using" statements).
/// In Main(), dispose subscribers before clearing the list (or use "using" statements).
/// </summary>

public class EventSubscriber : IDisposable
{
    private readonly string name;
    private readonly EventPublisher publisher;
    private bool disposed = false;

    public EventSubscriber(string name, EventPublisher publisher)
    {
        this.name = name;
        this.publisher = publisher;
        this.publisher.OnEvent += HandleEvent;
    }

    private void HandleEvent(object? sender, string message)
    {
        if (!this.disposed)
        {
            Console.WriteLine($"[{this.name}] Received:  {message}");
        }
    }

    // SOLUTION: Implement IDisposable to unsubscribe from the event.
    public void Dispose()
    {
        if (!this.disposed)
        {
            // Unsubscribe from the event to break the reference chain.
            this.publisher.OnEvent -= HandleEvent;
            this.disposed = true;
        }
    }
}

// SOLUTION: Updated Main to dispose subscribers.
public class Program
{
    public static void Main()
    {
        var publisher = new EventPublisher();
        var subscribers = new List<EventSubscriber>();

        Console.WriteLine("Creating subscribers...");

        for (int i = 0; i < 1000; i++)
        {
            var subscriber = new EventSubscriber($"Subscriber [{i}]", publisher);
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