using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class QueenTests : PieceTestBase
{
    [Theory]
    [InlineData(0, 0, "1,0|2,0|3,0|4,0|5,0|6,0|7,0|0,1|0,2|0,3|0,4|0,5|0,6|0,7|1,1|2,2|3,3|4,4|5,5|6,6|7,7")]
    [InlineData(Constants.BottomRank, Constants.RightmostFile, "6,7|5,7|4,7|3,7|2,7|1,7|0,7|7,6|7,5|7,4|7,3|7,2|7,1|7,0|6,6|5,5|4,4|3,3|2,2|1,1|0,0")]
    [InlineData(Constants.BottomRank, 0, "6,0|5,0|4,0|3,0|2,0|1,0|0,0|7,1|7,2|7,3|7,4|7,5|7,6|7,7|6,1|5,2|4,3|3,4|2,5|1,6|0,7")]
    [InlineData(0, Constants.RightmostFile, "0,6|0,5|0,4|0,3|0,2|0,1|0,0|1,7|2,7|3,7|4,7|5,7|6,7|7,7|1,6|2,5|3,4|4,3|5,2|6,1|7,0")]
    [InlineData(3, 3, "2,3|1,3|0,3|4,3|5,3|6,3|7,3|3,2|3,1|3,0|3,4|3,5|3,6|3,7|2,2|1,1|0,0|2,4|1,5|0,6|4,2|5,1|6,0|4,4|5,5|6,6|7,7")]
    public void QueenReturnsCorrectMovesOnEmptyBoard(int rank, int file, string expected)
    {
        var queen = new Queen(Colour.White);

        AssertAllExpectedMovesAreReturned(queen, rank, file, expected);
    }
}