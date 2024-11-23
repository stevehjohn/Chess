﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Engine.General;

namespace Engine.ConsoleTests;

[ExcludeFromCodeCoverage]
public static class EntryPoint
{
    private static readonly List<long> ExpectedCombinations =
    [
        20,
        400,
        8_902,
        197_281,
        4_865_609,
        119_060_324,
        3_195_901_860,
        84_998_978_956,
        2_439_530_234_167
    ];

    private static readonly Dictionary<(int Ply, PlyOutcome Outcome), long> ExpectedOutcomes = new()
    {
        { (1, PlyOutcome.Capture), 0 },
        { (1, PlyOutcome.EnPassant), 0 },
        { (1, PlyOutcome.Castle), 0 },
        { (1, PlyOutcome.Promotion), 0 },
        { (1, PlyOutcome.Check), 0 },
        { (1, PlyOutcome.CheckMate), 0 },
        
        { (2, PlyOutcome.Capture), 0 },
        { (2, PlyOutcome.EnPassant), 0 },
        { (2, PlyOutcome.Castle), 0 },
        { (2, PlyOutcome.Promotion), 0 },
        { (2, PlyOutcome.Check), 0 },
        { (2, PlyOutcome.CheckMate), 0 },
        
        { (3, PlyOutcome.Capture), 34 },
        { (3, PlyOutcome.EnPassant), 0 },
        { (3, PlyOutcome.Castle), 0 },
        { (3, PlyOutcome.Promotion), 0 },
        { (3, PlyOutcome.Check), 12 },
        { (3, PlyOutcome.CheckMate), 0 },
        
        { (4, PlyOutcome.Capture), 1_576 },
        { (4, PlyOutcome.EnPassant), 0 },
        { (4, PlyOutcome.Castle), 0 },
        { (4, PlyOutcome.Promotion), 0 },
        { (4, PlyOutcome.Check), 469 },
        { (4, PlyOutcome.CheckMate), 8 },
        
        { (5, PlyOutcome.Capture), 82_719 },
        { (5, PlyOutcome.EnPassant), 258 },
        { (5, PlyOutcome.Castle), 0 },
        { (5, PlyOutcome.Promotion), 0 },
        { (5, PlyOutcome.Check), 27_351 },
        { (5, PlyOutcome.CheckMate), 347 },
        
        { (6, PlyOutcome.Capture), 2_812_008 },
        { (6, PlyOutcome.EnPassant), 5_248 },
        { (6, PlyOutcome.Castle), 0 },
        { (6, PlyOutcome.Promotion), 0 },
        { (6, PlyOutcome.Check), 809_099 },
        { (6, PlyOutcome.CheckMate), 10_828 },
        
        { (7, PlyOutcome.Capture), 108_329_926 },
        { (7, PlyOutcome.EnPassant), 319_617 },
        { (7, PlyOutcome.Castle), 883_453 },
        { (7, PlyOutcome.Promotion), 0 },
        { (7, PlyOutcome.Check), 33_103_848 },
        { (7, PlyOutcome.CheckMate), 0435_767 },
        
        { (8, PlyOutcome.Capture), 3_523_740_106 },
        { (8, PlyOutcome.EnPassant), 7_187_977 },
        { (8, PlyOutcome.Castle), 23_605_205 },
        { (8, PlyOutcome.Promotion), 0 },
        { (8, PlyOutcome.Check), 968_981_593 },
        { (8, PlyOutcome.CheckMate), 9_852_036 },
        
        { (9, PlyOutcome.Capture), 125_208_536_153 },
        { (9, PlyOutcome.EnPassant), 319_496_827 },
        { (9, PlyOutcome.Castle), 1_784_356_000 },
        { (9, PlyOutcome.Promotion), 17_334_376 },
        { (9, PlyOutcome.Check), 36_095_901_903 },
        { (9, PlyOutcome.CheckMate), 400_191_963 }
    };
    
    public static void Main(string[] arguments)
    {
        var depth = 6;

        var perft = false;
        
        if (arguments.Length > 0)
        {
            int.TryParse(arguments[0], out depth);
        }

        if (arguments.Length > 1)
        {
            if (arguments[1].Equals("perft", StringComparison.InvariantCultureIgnoreCase))
            {
                perft = true;
            }
        }

        Console.WriteLine();

        for (var i = 1; i <= depth; i++)
        {
            var core = new Core();

            core.Initialise();

            Console.WriteLine($"  {DateTime.Now:HH:mm:ss} Starting depth {i}");
            
            Console.WriteLine();
            
            var stopwatch = Stopwatch.StartNew();

            core.GetMove(i);

            stopwatch.Stop();

            for (var j = 1; j <= i; j++)
            {
                var count = core.GetDepthCount(j);

                var expected = ExpectedCombinations[j - 1];
                
                var pass = count == expected;
                
                Console.Write($"  {(pass ? "✓ PASS" : "  FAIL")}  Depth: {j,2}  Combinations: {count,15:N0}  Expected: {expected,15:N0}");
                
                if (! pass)
                {
                    var delta = count - expected;
                    
                    Console.Write($"  Delta: {(delta > 0 ? ">" : "<")}{delta,13:N0}");
                }
                
                Console.WriteLine();
                
                Console.Write($"      Capture:    {core.GetPlyOutcome(j, PlyOutcome.Capture),13:N0}");
                Console.Write($" {(ExpectedOutcomes[(j, PlyOutcome.Capture)] == core.GetPlyOutcome(j, PlyOutcome.Capture) ? "✓" : string.Empty)}");
                if (ExpectedOutcomes[(j, PlyOutcome.Capture)] == core.GetPlyOutcome(j, PlyOutcome.Capture))
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Delta: {core.GetPlyOutcome(j, PlyOutcome.Capture) - ExpectedOutcomes[(j, PlyOutcome.Capture)],13:N0}");
                }
                
                Console.Write($"      En Passant: {core.GetPlyOutcome(j, PlyOutcome.EnPassant),13:N0}");
                Console.Write($" {(ExpectedOutcomes[(j, PlyOutcome.EnPassant)] == core.GetPlyOutcome(j, PlyOutcome.EnPassant) ? "✓" : string.Empty)}");
                if (ExpectedOutcomes[(j, PlyOutcome.EnPassant)] == core.GetPlyOutcome(j, PlyOutcome.EnPassant))
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Delta: {core.GetPlyOutcome(j, PlyOutcome.EnPassant) - ExpectedOutcomes[(j, PlyOutcome.EnPassant)],13:N0}");
                }
                
                Console.Write($"      Castle:     {core.GetPlyOutcome(j, PlyOutcome.Castle),13:N0}");
                Console.Write($" {(ExpectedOutcomes[(j, PlyOutcome.Castle)] == core.GetPlyOutcome(j, PlyOutcome.Castle) ? "✓" : string.Empty)}");
                if (ExpectedOutcomes[(j, PlyOutcome.Castle)] == core.GetPlyOutcome(j, PlyOutcome.Castle))
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Delta: {core.GetPlyOutcome(j, PlyOutcome.Castle) - ExpectedOutcomes[(j, PlyOutcome.Castle)],13:N0}");
                }
                
                Console.Write($"      Promotion:  {core.GetPlyOutcome(j, PlyOutcome.Promotion),13:N0}");
                Console.Write($" {(ExpectedOutcomes[(j, PlyOutcome.Promotion)] == core.GetPlyOutcome(j, PlyOutcome.Promotion) ? "✓" : string.Empty)}");
                if (ExpectedOutcomes[(j, PlyOutcome.Promotion)] == core.GetPlyOutcome(j, PlyOutcome.Promotion))
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Delta: {core.GetPlyOutcome(j, PlyOutcome.Promotion) - ExpectedOutcomes[(j, PlyOutcome.Promotion)],13:N0}");
                }
                
                Console.Write($"      Check:      {core.GetPlyOutcome(j, PlyOutcome.Check),13:N0}");
                Console.Write($" {(ExpectedOutcomes[(j, PlyOutcome.Check)] == core.GetPlyOutcome(j, PlyOutcome.Check) ? "✓" : string.Empty)}");
                if (ExpectedOutcomes[(j, PlyOutcome.Check)] == core.GetPlyOutcome(j, PlyOutcome.Check))
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Delta: {core.GetPlyOutcome(j, PlyOutcome.Check) - ExpectedOutcomes[(j, PlyOutcome.Check)],13:N0}");
                }
                
                Console.Write($"      Check Mate: {core.GetPlyOutcome(j, PlyOutcome.CheckMate),13:N0}");
                Console.Write($" {(ExpectedOutcomes[(j, PlyOutcome.CheckMate)] == core.GetPlyOutcome(j, PlyOutcome.CheckMate) ? "✓" : string.Empty)}");
                if (ExpectedOutcomes[(j, PlyOutcome.CheckMate)] == core.GetPlyOutcome(j, PlyOutcome.CheckMate))
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Delta: {core.GetPlyOutcome(j, PlyOutcome.CheckMate) - ExpectedOutcomes[(j, PlyOutcome.CheckMate)],13:N0}");
                }
                
                Console.WriteLine($"  Best Score: {core.GetBestScore(j)}    BestMoveCount: {core.GetBestMoveCount()}");

                if (perft)
                {
                    if (j == i)
                    {
                        Console.WriteLine();

                        foreach (var node in core.PerftCounts.OrderBy(n => n.Key))
                        {
                            Console.WriteLine($"  {node.Key}: {node.Value:N0}");
                        }
                    }
                }
            }

            Console.WriteLine();

            Console.WriteLine($"  {i} depth{(i > 1 ? "s" : string.Empty)} explored in {(stopwatch.Elapsed.Hours > 0 ? $"{stopwatch.Elapsed.Hours}h " : string.Empty)}{stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds:N0}s {stopwatch.Elapsed.Milliseconds}ms");

            Console.WriteLine();
        }
    }
}