using System.Text;
using Engine.General;
using Engine.Pieces;
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
    
    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 400)]
    [InlineData(3, 8_902)]
    [InlineData(4, 197_281)]
    [InlineData(5, 4_865_609)]
    [InlineData(6, 119_060_324)]
    public void ExploresExpectedNumberOfCombinations(int depth, int expected)
    {
        _board.Initialise();
        
        var result = _core.GetMove(Kind.White, depth);
        
        var output = $"{(result.Combinations == expected ? "PASS" : "FAIL")} Depth: {depth}, Explored: {result.Combinations}, Expected: {expected}";
        
        _outputHelper.WriteLine(output);
        
        // ReSharper disable once Xunit.XunitTestWithConsoleOutput - output doesn't show from dotnet test otherwise
        Console.WriteLine(output);
    }
}