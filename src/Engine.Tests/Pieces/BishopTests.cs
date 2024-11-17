using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class BishopTests : PieceTestBase
{
    [Theory]
    [InlineData(0, 0, "1,1|2,2|3,3|4,4|5,5|6,6|7,7")]
    [InlineData(Constants.RightmostRank, Constants.BottomFile, "1,1|2,2|3,3|4,4|5,5|6,6|7,7")]
    [InlineData(Constants.RightmostRank, 0, "6,1|5,2|4,3|3,4|2,5|1,6|0,7")]
    [InlineData(0, Constants.BottomFile, "6,1|5,2|4,3|3,4|2,5|1,6|0,7")]
    [InlineData(3, 3, "2,2|1,1|0,0|2,4|1,5|0,6|4,2|5,1|6,0|4,4|5,5|6,6|7,7")]
    public void BishopReturnsCorrectMovesOnEmptyBoard(int rank, int file, string expected)
    {
        var bishop = new Bishop(Colour.White);

        AssertAllExpectedMovesAreReturned(bishop, rank, file, expected);
    }
}