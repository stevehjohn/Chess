using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class KingTests : PieceTestBase
{
    [Theory]
    [InlineData(0, 0, "1,0|0,1")]
    [InlineData(Constants.BottomRank, Constants.RightmostFile, "6,7|7,6")]
    [InlineData(Constants.BottomRank, 0, "7,1|6,0")]
    [InlineData(0, Constants.RightmostFile, "0,1|1,0")]
    [InlineData(3, 3, "2,3|4,3|3,2|3,4")]
    public void KingReturnsCorrectMovesOnEmptyBoard(int rank, int file, string expected)
    {
        var king = new King(Colour.White);

        AssertAllExpectedMovesAreReturned(king, rank, file, expected);
    }
}