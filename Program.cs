using System;
using System.Buffers;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== GC Pressure Demo ===");

        RunTest("Without Pooling", usePooling: false);
        Console.WriteLine();
        RunTest("With ArrayPool", usePooling: true);

        Console.WriteLine("\nDone.");
    }

    static void RunTest(string label, bool usePooling)
    {
        Console.WriteLine($"--- {label} ---");

        // Track GC stats before
        long startAllocated = GC.GetTotalAllocatedBytes(true);
        int gc0Start = GC.CollectionCount(0);
        int gc1Start = GC.CollectionCount(1);
        int gc2Start = GC.CollectionCount(2);

        var sw = Stopwatch.StartNew();

        for (int i = 0; i < 50_000; i++)
        {
            int size = 1024; // 1 KB arrays

            byte[] buffer;

            if (usePooling)
            {
                buffer = ArrayPool<byte>.Shared.Rent(size);

                // Simulate work
                buffer[0] = 123;

                ArrayPool<byte>.Shared.Return(buffer);
            }
            else
            {
                buffer = new byte[size];
                buffer[0] = 123;
            }
        }

        sw.Stop();

        // Track GC stats after
        long endAllocated = GC.GetTotalAllocatedBytes(true);
        int gc0End = GC.CollectionCount(0);
        int gc1End = GC.CollectionCount(1);
        int gc2End = GC.CollectionCount(2);

        // Print results
        Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"Allocated bytes: {endAllocated - startAllocated:N0}");
        Console.WriteLine($"GC Gen0: {gc0End - gc0Start}");
        Console.WriteLine($"GC Gen1: {gc1End - gc1Start}");
        Console.WriteLine($"GC Gen2: {gc2End - gc2Start}");
    }
}
