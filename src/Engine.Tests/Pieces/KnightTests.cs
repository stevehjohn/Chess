using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class KnightTests : PieceTestBase
{
    [Theory]
    [InlineData(3, 3, "5,2|5,4|1,2|1,4|4,1|2,1|4,5|2,5")]
    [InlineData(7, 0, "5,1|6,2")]
    public void KnightReturnsCorrectMovesOnEmptyBoard(int rank, int file, string expected)
    {
        var knight = new Knight(Colour.White);

        AssertAllExpectedMovesAreReturned(knight, rank, file, expected);
    }
}