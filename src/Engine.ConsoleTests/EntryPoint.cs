using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    
    public static void Main(string[] arguments)
    {
        var depth = 8;
        
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
                
                Console.Write($"  {(pass ? "✓ PASS" : "  FAIL")}  Depth: {j,2}  Combinations: {count,13:N0}  Expected: {expected,13:N0}");

                if (! pass)
                {
                    Console.Write($"  Delta: {Math.Abs(count - expected),11:N0}");
                }
                
                Console.WriteLine();

                if (j == i)
                {
                    Console.WriteLine();
                    
                    foreach (var node in core.PerftCounts.OrderBy(n => n.Key))
                    {
                        Console.WriteLine($"  {node.Key}: {node.Value}");
                    }
                }
            }

            Console.WriteLine();

            Console.WriteLine($"  {i} depth{(i > 1 ? "s" : string.Empty)} explored in {stopwatch.Elapsed.Minutes}m {stopwatch.Elapsed.Seconds:N0}s {stopwatch.Elapsed.Milliseconds}ms");

            Console.WriteLine();
        }
    }
}