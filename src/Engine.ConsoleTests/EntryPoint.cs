using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Engine.General;
using Engine.Pieces;

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
        2_439_530_234_167,
        69_352_859_712_417
    ];

    private static readonly Dictionary<(int Ply, PlyOutcome Outcome), long> ExpectedOutcomes = new()
    {
        { (1, PlyOutcome.Capture), 0 },
        { (1, PlyOutcome.EnPassant), 0 },
        { (1, PlyOutcome.Castle), 0 },
        { (1, PlyOutcome.Check), 0 },
        { (2, PlyOutcome.Capture), 0 },
        { (2, PlyOutcome.EnPassant), 0 },
        { (2, PlyOutcome.Castle), 0 },
        { (2, PlyOutcome.Check), 0 },
        { (3, PlyOutcome.Capture), 34 },
        { (3, PlyOutcome.EnPassant), 0 },
        { (3, PlyOutcome.Castle), 0 },
        { (3, PlyOutcome.Check), 12 },
        { (4, PlyOutcome.Capture), 1_576 },
        { (4, PlyOutcome.EnPassant), 0 },
        { (4, PlyOutcome.Castle), 0 },
        { (4, PlyOutcome.Check), 469 },
        { (5, PlyOutcome.Capture), 82_719 },
        { (5, PlyOutcome.EnPassant), 258 },
        { (5, PlyOutcome.Castle), 0 },
        { (5, PlyOutcome.Check), 27_351 },
        { (6, PlyOutcome.Capture), 2_812_008 },
        { (6, PlyOutcome.EnPassant), 5_248 },
        { (6, PlyOutcome.Castle), 0 },
        { (6, PlyOutcome.Check), 809_099 },
        { (7, PlyOutcome.Capture), 108_329_926 },
        { (7, PlyOutcome.EnPassant), 319_617 },
        { (7, PlyOutcome.Castle), 883_453 },
        { (7, PlyOutcome.Check), 33_103_848 },
        { (8, PlyOutcome.Capture), 0 },
        { (8, PlyOutcome.EnPassant), 0 },
        { (8, PlyOutcome.Castle), 0 },
        { (8, PlyOutcome.Check), 0 },
        { (9, PlyOutcome.Capture), 0 },
        { (9, PlyOutcome.EnPassant), 0 },
        { (9, PlyOutcome.Castle), 0 },
        { (9, PlyOutcome.Check), 0 },
        { (10, PlyOutcome.Capture), 0 },
        { (10, PlyOutcome.EnPassant), 0 },
        { (10, PlyOutcome.Castle), 0 },
        { (10, PlyOutcome.Check), 0 }
    };
    
    public static void Main(string[] arguments)
    {
        var depth = 6;
        
        if (arguments.Length > 0)
        {
            int.TryParse(arguments[0], out depth);
        }
        
        Console.WriteLine();

        for (var i = 1; i <= depth; i++)
        {
            var core = new Core();

            core.Initialise();

            Console.WriteLine($"  {DateTime.Now:HH:mm:ss} Starting depth {i}");
            
            Console.WriteLine();
            
            var stopwatch = Stopwatch.StartNew();

            core.GetMove(Colour.White, i);

            stopwatch.Stop();

            for (var j = 1; j <= i; j++)
            {
                var count = core.DepthCounts[j];

                var expected = ExpectedCombinations[j - 1];
                
                var pass = count == expected;
                
                Console.Write($"  {(pass ? "✓ PASS" : "  FAIL")}  Depth: {j,2}  Combinations: {count,14:N0}  Expected: {expected,14:N0}");
                
                if (! pass)
                {
                    var delta = count - expected;
                    
                    Console.Write($"  Delta: {(delta > 0 ? ">" : "<")}{delta,13:N0}");
                }
                
                Console.WriteLine();
                
                Console.Write($"      Captures:   {core.Outcomes[(j, PlyOutcome.Capture)],13:N0}");
                Console.WriteLine($" {(ExpectedOutcomes[(j, PlyOutcome.Capture)] == core.Outcomes[(j, PlyOutcome.Capture)] ? "✓" : string.Empty)}");
                Console.Write($"      En Passant: {core.Outcomes[(j, PlyOutcome.EnPassant)],13:N0}");
                Console.Write($"      Castle:     {core.Outcomes[(j, PlyOutcome.Castle)],13:N0}");
                Console.Write($"      Check:      {core.Outcomes[(j, PlyOutcome.Check)],13:N0}");
            }

            Console.WriteLine();

            Console.WriteLine($"  {i} depth{(i > 1 ? "s" : string.Empty)} explored in {(stopwatch.Elapsed.Hours > 0 ? $"{stopwatch.Elapsed.Hours}h" : string.Empty)}{stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds:N0}s {stopwatch.Elapsed.Milliseconds}ms");

            Console.WriteLine();
        }
    }
}