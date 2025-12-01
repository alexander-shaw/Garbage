// <copyright file="AlexDemo.cs" company="Alex">
// Copyright (c) Alex. All rights reserved.
// </copyright>

using System;
using System.Threading;

namespace AlexDemo;

/// <summary>
/// Event Leak Demo.
/// </summary>
public class AlexDemo
{
    /// <summary>
    /// Runs the demo with a menu to choose between leaky and fixed versions.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("ALEX'S DEMO");
        Console.WriteLine();
        Console.WriteLine("1  |  Leaky version");
        Console.WriteLine("2  |  Fixed version");
        Console.WriteLine("0  |  Exit");
        Console.Write("SELECT:  ");

        string? choice = Console.ReadLine();
        Console.WriteLine();

        if (choice == "1")
        {
            RunLeaky();
        } 
        else if (choice == "2")
        {
            RunFixed();
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
    /// Runs the leaky version that demonstrates memory growth and (potential) crash.
    /// </summary>
    static void RunLeaky()
    {
        Console.WriteLine("LEAKY VERSION");
        Console.WriteLine("Memory will grow and crash!");
        
        int count = 0;
        while (true)
        {
            try
            {
                var listener = new LeakyListener(count);
                count++;

                if (count % 100 == 0)
                {
                    GlobalTimer.RaiseTick();
                    long mem = GC.GetTotalMemory(false);
                    Console.WriteLine($"Created {count} listeners.  Memory: {mem / (1024 * 1024)} MB");
                }

                Thread.Sleep(5);
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("OutOfMemoryException!  Too many listeners leaked!");
                break;
            }
        }
    }

    /// <summary>
    /// Runs the fixed version that properly disposes listeners.
    /// </summary>
    static void RunFixed()
    {
        Console.WriteLine("FIXED VERSION");
        Console.WriteLine("Memory stays stable.");
        int count = 0;

        while (true)
        {
            using (var listener = new SafeListener(count))
            {
                count++;

                if (count % 100 == 0)
                {
                    GlobalTimer.RaiseTick();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    long mem = GC.GetTotalMemory(false);
                    Console.WriteLine($"Created {count} listeners.  Memory: {mem / (1024 * 1024)} MB");
                }

                Thread.Sleep(5);
            }
        }
    }
}