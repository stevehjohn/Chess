using Engine.General;
using Engine.Pieces;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Engine.ConsoleTests
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const int Depth = 7;

        public static void Main()
        {
            for (var j = 0; j < 2; j++)
            {
                Console.WriteLine(j == 0 ? "Concurrent" : "\nSequential");

                for (var i = 1; i < Depth; i++)
                {
                    Console.WriteLine($"\nDepth: {i}\n");

                    var board = BoardBuilder.Build();
                    var engine = new ChessEngine(board, i, j == 0);

                    var stopwatch = Stopwatch.StartNew();

                    engine.GetMove(Side.White);

                    stopwatch.Stop();

                    Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds / 1000.0}");

                    for (var k = 0; k < Depth; k++)
                    {
                        var max = engine.Depths[k].Max(m => m.TotalValue);
                        var count = engine.Depths[k].Count(m => m.TotalValue == max);

                        Console.WriteLine($"Depth {k}, Max Score {max}, Count of Max Score Nodes {count}, Total Nodes: {engine.Depths[k].Count}");
                    }
                }
            }

            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }
    }
}
