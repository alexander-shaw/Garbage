// <copyright file="Program.cs" company="Garbagemen">
// Copyright (c) Garbagemen. All rights reserved.
// </copyright>

using VijayDemo;
using AlexDemo;
using IshaanDemo;
using NickDemo;

// Main menu loop.
while (true)
{
    Console.WriteLine();
    Console.WriteLine("MEMORY DEMOS");
    Console.WriteLine("1  |  Vijay's Demo");
    Console.WriteLine("2  |  Alex's Demo");
    Console.WriteLine("3  |  Ishaan's Demo");
    Console.WriteLine("4  |  Nick's Demo");
    Console.WriteLine("0  |  Exit");
    Console.Write("SELECT:  ");

    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input == "0")
    {
        Console.WriteLine("Thanks!  Have a good one!");
        break;
    }

    Console.WriteLine();

    switch (input)
    {
        case "1":
            VijayDemo.VijayDemo.Run();
            break;
        case "2":
            AlexDemo.AlexDemo.Run();
            break;
        case "3":
            IshaanDemo.IshaanDemo.Run();
            break;
        case "4":
            NickDemo.NickDemo.Run();
            break;
        default:
            Console.WriteLine("INVALID: PLEASE TRY AGAIN.");
            break;
    }
}