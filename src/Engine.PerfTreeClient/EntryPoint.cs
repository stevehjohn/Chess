using Engine.Extensions;

namespace Engine.PerfTreeClient;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        var consoleColour = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Magenta;
        
        foreach (var argument in arguments)
        {
            Console.Error.WriteLine(argument);
        }

        Console.ForegroundColor = consoleColour;
        
        var core = new Core();

        var depth = int.Parse(arguments[0]);

        var fen = arguments[1];
        
        core.Initialise(fen);

        if (arguments.Length > 2)
        {
            var moves = arguments[2].Split(' ');

            foreach (var move in moves)
            {
                var position = move[..2].CellFromStandardNotation();

                var target = move[2..].CellFromStandardNotation();
            
                core.MakeMove(position, target);
            
                Console.Error.WriteLine($"Moving {move[..2]} ({position}) to {move[2..]} ({target})");
            }
        }

        core.GetMove(depth);
        
        foreach (var node in core.PerftCounts.OrderBy(n => n.Key))
        {
            Console.WriteLine($"{node.Key} {node.Value}");
        }
        
        Console.WriteLine();
        
        Console.WriteLine(core.GetDepthCount(depth));
    }
}