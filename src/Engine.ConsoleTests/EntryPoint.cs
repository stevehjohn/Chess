using System.Diagnostics;
using Engine.Pieces;

namespace Engine.ConsoleTests;

public static class EntryPoint
{
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

            for (var j = 1; j < depth; j++)
            {
                Console.WriteLine($"  Depth: {i,2}    Combinations: {core.DepthCounts[j],13:N0}    Expected: ");
            }

            Console.WriteLine();

            Console.WriteLine($"  {i} depths explored in {stopwatch.Elapsed:g}");

            Console.WriteLine();
        }
    }
}