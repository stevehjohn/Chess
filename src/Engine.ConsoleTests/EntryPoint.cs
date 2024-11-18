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
        
        core.GetMove(Colour.White, 6);

        for (var i = 1; i < Depth; i++)
        {
            Console.WriteLine($"  Depth: {i,2}    Combinations: {core.DepthCounts[i],13:N0}    Expected: ");
        }
    }
}