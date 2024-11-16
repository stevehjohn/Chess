using Engine.General;
using Xunit;

namespace Engine.Tests;

public class CoreTests
{
    private readonly Board _board;

    private readonly Core _core;

    public CoreTests()
    {
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
        
    }
}