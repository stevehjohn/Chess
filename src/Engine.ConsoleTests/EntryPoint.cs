using System.Diagnostics;
using Engine.Pieces;

namespace Engine.ConsoleTests;

public static class EntryPoint
{
    private const int Depth = 6;
    
    public static void Main()
    {
        var core = new Core();
        
        core.Initialise();
        
        Console.Clear();
        
        Console.WriteLine();

        var stopwatch = Stopwatch.StartNew();
        
        core.GetMove(Colour.White, 6);
        
        stopwatch.Stop();

        for (var i = 1; i < Depth; i++)
        {
            Console.WriteLine($"  Depth: {i,2}    Combinations: {core.DepthCounts[i],13:N0}    Expected: ");
        }
        
        Console.WriteLine();
        
        Console.WriteLine($"  {Depth} depths explored in {stopwatch.Elapsed:g}");
        
        Console.WriteLine();
    }
}