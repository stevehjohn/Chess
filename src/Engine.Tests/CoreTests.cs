using Engine.Pieces;
using Xunit;
using Xunit.Abstractions;

namespace Engine.Tests;

public class CoreTests
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
            var output = $"Depth: {i,2}    Combinations: {_core.DepthCounts[i],13:N0}    Expected: ";
            
            _outputHelper.WriteLine(output);
            
            // ReSharper disable once Xunit.XunitTestWithConsoleOutput
            Console.WriteLine(output);
        }
    }
}