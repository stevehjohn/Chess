using System.Diagnostics;
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
        var depth = 5;

        var perft = false;
        
        if (arguments.Length > 0)
        {
            if (! int.TryParse(arguments[0], out depth))
            {
                depth = 6;
            }
        }

        if (arguments.Length > 1)
        {
            if (arguments[1].Equals("perft", StringComparison.InvariantCultureIgnoreCase))
            {
                perft = true;
            }
        }

        Console.WriteLine();

        for (var maxDepth = 1; maxDepth <= depth; maxDepth++)
        {
            var core = new Core();

            core.Initialise();

            Console.WriteLine($"  {DateTime.Now:HH:mm:ss} Starting depth {maxDepth}");
            
            Console.WriteLine();
            
            var stopwatch = Stopwatch.StartNew();

            var exception = false;
            
            // ReSharper disable once AccessToModifiedClosure
            core.GetMove(maxDepth, _ => PlyComplete(core, maxDepth, stopwatch, perft))
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        exception = true;
                        
                        Console.WriteLine(task.Exception);
                    }
                });

            var y = Console.CursorLeft;
            
            while (core.IsBusy && ! exception)
            {
                Thread.Sleep(1000);

                if (core.IsBusy)
                {
                    var depthCount = core.GetDepthCount(maxDepth);
                    
                    var percent = (float) depthCount / ExpectedCombinations[maxDepth - 1] * 100;

                    var averagePerSecond = stopwatch.Elapsed.TotalSeconds / depthCount;

                    var remaining = ExpectedCombinations[maxDepth - 1] - depthCount;

                    var timeRemaining = TimeSpan.FromSeconds(remaining * averagePerSecond);

                    var etr = $"{(timeRemaining.Days > 0 ? $"{timeRemaining.Days:N0}d" : string.Empty)}{timeRemaining.Hours,2:00}:{timeRemaining.Minutes,2:00}.{timeRemaining.Seconds % 60,2:00}";
                    
                    Console.Write($"  {DateTime.Now:HH:mm:ss} {percent:N2}% {depthCount:N0} / {ExpectedCombinations[maxDepth - 1]:N0} ETR: {etr}");

                    Console.CursorLeft = y;
                }
            }

            if (exception)
            {
                break;
            }
        }
    }

    private static void PlyComplete(Core core, int maxDepth, Stopwatch stopwatch, bool perft)
    {
        stopwatch.Stop();

        for (var depth = 1; depth <= maxDepth; depth++)
        {
            var count = core.GetDepthCount(depth);

            var expected = ExpectedCombinations[depth - 1];

            var pass = count == expected;

            Console.Write($"  {(pass ? "✓ PASS" : "  FAIL")}  Depth: {depth,2}  Combinations: {count,15:N0}  Expected: {expected,15:N0}");

            if (! pass)
            {
                var delta = count - expected;

                Console.Write($"  Delta: {(delta > 0 ? ">" : "<")}{delta,13:N0}");
            }

            Console.WriteLine();

            Console.Write($"      Capture:    {core.GetPlyOutcome(depth, PlyOutcome.Capture),13:N0}");
            Console.Write($" {(ExpectedOutcomes[(depth, PlyOutcome.Capture)] == core.GetPlyOutcome(depth, PlyOutcome.Capture) ? "✓" : string.Empty)}");
            if (ExpectedOutcomes[(depth, PlyOutcome.Capture)] == core.GetPlyOutcome(depth, PlyOutcome.Capture))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"  Delta: {core.GetPlyOutcome(depth, PlyOutcome.Capture) - ExpectedOutcomes[(depth, PlyOutcome.Capture)],13:N0}");
            }

            Console.Write($"      En Passant: {core.GetPlyOutcome(depth, PlyOutcome.EnPassant),13:N0}");
            Console.Write($" {(ExpectedOutcomes[(depth, PlyOutcome.EnPassant)] == core.GetPlyOutcome(depth, PlyOutcome.EnPassant) ? "✓" : string.Empty)}");
            if (ExpectedOutcomes[(depth, PlyOutcome.EnPassant)] == core.GetPlyOutcome(depth, PlyOutcome.EnPassant))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"  Delta: {core.GetPlyOutcome(depth, PlyOutcome.EnPassant) - ExpectedOutcomes[(depth, PlyOutcome.EnPassant)],13:N0}");
            }

            Console.Write($"      Castle:     {core.GetPlyOutcome(depth, PlyOutcome.Castle),13:N0}");
            Console.Write($" {(ExpectedOutcomes[(depth, PlyOutcome.Castle)] == core.GetPlyOutcome(depth, PlyOutcome.Castle) ? "✓" : string.Empty)}");
            if (ExpectedOutcomes[(depth, PlyOutcome.Castle)] == core.GetPlyOutcome(depth, PlyOutcome.Castle))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"  Delta: {core.GetPlyOutcome(depth, PlyOutcome.Castle) - ExpectedOutcomes[(depth, PlyOutcome.Castle)],13:N0}");
            }

            Console.Write($"      Promotion:  {core.GetPlyOutcome(depth, PlyOutcome.Promotion),13:N0}");
            Console.Write($" {(ExpectedOutcomes[(depth, PlyOutcome.Promotion)] == core.GetPlyOutcome(depth, PlyOutcome.Promotion) ? "✓" : string.Empty)}");
            if (ExpectedOutcomes[(depth, PlyOutcome.Promotion)] == core.GetPlyOutcome(depth, PlyOutcome.Promotion))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"  Delta: {core.GetPlyOutcome(depth, PlyOutcome.Promotion) - ExpectedOutcomes[(depth, PlyOutcome.Promotion)],13:N0}");
            }

            Console.Write($"      Check:      {core.GetPlyOutcome(depth, PlyOutcome.Check),13:N0}");
            Console.Write($" {(ExpectedOutcomes[(depth, PlyOutcome.Check)] == core.GetPlyOutcome(depth, PlyOutcome.Check) ? "✓" : string.Empty)}");
            if (ExpectedOutcomes[(depth, PlyOutcome.Check)] == core.GetPlyOutcome(depth, PlyOutcome.Check))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"  Delta: {core.GetPlyOutcome(depth, PlyOutcome.Check) - ExpectedOutcomes[(depth, PlyOutcome.Check)],13:N0}");
            }

            Console.Write($"      Check Mate: {core.GetPlyOutcome(depth, PlyOutcome.CheckMate),13:N0}");
            Console.Write($" {(ExpectedOutcomes[(depth, PlyOutcome.CheckMate)] == core.GetPlyOutcome(depth, PlyOutcome.CheckMate) ? "✓" : string.Empty)}");
            if (ExpectedOutcomes[(depth, PlyOutcome.CheckMate)] == core.GetPlyOutcome(depth, PlyOutcome.CheckMate))
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"  Delta: {core.GetPlyOutcome(depth, PlyOutcome.CheckMate) - ExpectedOutcomes[(depth, PlyOutcome.CheckMate)],13:N0}");
            }

            Console.WriteLine($"      Best Score: {core.GetBestScore(depth)}    BestMoveCount: {core.GetBestMoveCount(depth)}");

            if (perft)
            {
                if (depth == maxDepth)
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

        Console.WriteLine($"  {maxDepth} depth{(maxDepth > 1 ? "s" : string.Empty)} explored in {(stopwatch.Elapsed.Hours > 0 ? $"{stopwatch.Elapsed.Hours}h " : string.Empty)}{stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds:N0}s {stopwatch.Elapsed.Milliseconds}ms");

        Console.WriteLine();
    }
}