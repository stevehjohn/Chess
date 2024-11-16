using Engine.General;
using Engine.Pieces;
using Engine.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Engine.Tests;

public class CoreTests
{
    private readonly ITestOutputHelper _outputHelper;
    
    private readonly Board _board;

    private readonly Core _core;

    public CoreTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        
        _board = new Board();

        _core = new Core(_board);
    }
    
    [Fact]
    public void ExploresExpectedNumberOfCombinations()
    {
        for (var depth = 1; depth < 7; depth++)
        {
            _board.Initialise();
        
            var result = _core.GetMove(Kind.White, depth);

            var expected = depth switch
            {
                1 => 20,
                2 => 400,
                3 => 8_902,
                4 => 197_281,
                5 => 4_865_609,
                6 => 119_060_324,
                _ => throw new TestException($"Expected count not know for depth {depth}")
            };
        
            var output = $"{(result.Combinations == expected ? "PASS" : "FAIL")} Depth: {depth}, Explored: {result.Combinations}, Expected: {expected}";
        
            _outputHelper.WriteLine(output);
        
            // ReSharper disable once Xunit.XunitTestWithConsoleOutput - output doesn't show from dotnet test otherwise
            Console.WriteLine(output);
        }
    }
}