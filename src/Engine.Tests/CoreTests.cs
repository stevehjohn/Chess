using Engine.General;
using Xunit;

namespace Engine.Tests;

public class CoreTests
{
    private Board _board = new Board();

    [Theory]
    [InlineData(1, 20)]
    public void MovesPerPly(int plies, int expectedCombinations)
    {
    }
}