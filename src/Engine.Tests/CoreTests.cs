using Engine.Pieces;
using Xunit;
using Xunit.Abstractions;

namespace Engine.Tests;

public class CoreTests
{
    private readonly ITestOutputHelper _outputHelper;
    
    private readonly Core _core = new();

    public CoreTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Theory]
    [InlineData(3)]
    public void MovesPerPly(int depth)
    {
        _core.Initialise();

        _core.GetMove(Colour.White, depth);

        for (var i = 1; i <= depth; i++)
        {
            _outputHelper.WriteLine($"Depth: {i,2}    Combinations: {_core.DepthCounts[i],13:N0}    Expected: ");
        }
    }
}