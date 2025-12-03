// <copyright file="VijayDemo.cs" company="Vijay">
// Copyright (c) Vijay. All rights reserved.
// </copyright>

namespace VijayDemo;

/// <summary>
/// Demo class for Vijay.
/// </summary>
public class VijayDemo
{
    /// <summary>
    /// Runs the demo.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("VIJAY'S DEMO");

        // TODO: Implement your demo here.
    }

    public static void StackAllocation()
    {
        int x = 10;

        // Fast, Value types are allocated on the stack
    }

    public static void HeapAllocation()
    {
        var obj = new object();
        // Slower, Reference types are allocated on the heap
    }
}
