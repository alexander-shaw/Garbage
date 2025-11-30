// <copyright file="IMemoryDemo.cs" company="Garbagemen">
// Copyright (c) Garbagemen. All rights reserved.
// </copyright>

namespace MemDemos.Shared;

/// <summary>
/// Interface that all memory demo classes must implement.
/// Each demo provides both a buggy version and a fixed version of a memory-related issue.
/// </summary>
public interface IMemoryDemo
{
    /// <summary>
    /// Gets the unique identifier for this demo: 1, 2, 3, 4.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the human-readable title for this demo.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets a short explanation of the memory issue scenario being demonstrated.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Runs the buggy/leaky version of the demo.
    /// This should demonstrate the memory issue in action.
    /// </summary>
    void RunBugVersion();

    /// <summary>
    /// Runs the corrected/fixed version of the demo.
    /// This should demonstrate how the issue is resolved.
    /// </summary>
    void RunFixedVersion();
}