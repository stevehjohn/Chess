using System.Diagnostics;
using Engine.Pieces;

namespace Engine.ConsoleTests;

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
        var depth = 6;
        
        if (arguments.Length > 0)
        {
            int.TryParse(arguments[0], out depth);
        }
        
        Console.Clear();
        
        Console.WriteLine();


        for (var i = 1; i <= depth; i++)
        {
            var core = new Core();

            core.Initialise();

            var stopwatch = Stopwatch.StartNew();

            core.GetMove(Colour.White, i);

            stopwatch.Stop();

            for (var j = 1; j <= i; j++)
            {
                var count = core.DepthCounts[j];

                var expected = ExpectedCombinations[j - 1];
                
                var pass = count == expected;
                
                Console.Write($"  {(pass ? "PASS" : "FAIL")}  Depth: {j,2}  Combinations: {count,13:N0}  Expected: {expected,13:N0}");

                if (! pass)
                {
                    Console.Write($"  Delta: {expected - count,4:N0}");
                }
                
                Console.WriteLine();
            }

            Console.WriteLine();

            Console.WriteLine($"  {i} depths explored in {stopwatch.Elapsed.Seconds:N0}s {stopwatch.Elapsed.Milliseconds}ms");

            Console.WriteLine();
        }
    }
}