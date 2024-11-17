using Xunit;

namespace Engine.Tests;

public class CoreTests
{
    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 400)]
    public void MovesPerPly(int plies, int expectedCombinations)
    {
    }
}