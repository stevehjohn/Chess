using Engine.General;
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

    [Theory]
    [InlineData(Constants.WhiteHomeRank, Constants.LeftKnightFile, "5,0|5,2")]
    [InlineData(Constants.WhiteHomeRank, Constants.RightKnightFile, "5,5|5,7")]
    [InlineData(Constants.BlackHomeRank, Constants.LeftKnightFile, "2,0|2,2")]
    [InlineData(Constants.BlackHomeRank, Constants.RightKnightFile, "2,5|2,7")]
    public void KnightReturnsCorrectStartingMoves(int rank, int file, string expected)
    {
        Board.InitialisePieces();

        var knight = Board[rank, file];

        AssertAllExpectedMovesAreReturned(knight, rank, file, expected);
    }

    [Theory]
    [InlineData(3, 3, 5, 4, "5,2|1,2|1,4|4,1|2,1|4,5|2,5")]
    [InlineData(7, 0, 5, 1, "6,2")]
    public void KnightIsBlockedByOwnPieces(int rank, int file, int blockerRank, int blockerFile, string expected)
    {
        var knight = new Knight(Colour.White);

        Board[blockerRank, blockerFile] = new Pawn(Colour.White);

        AssertAllExpectedMovesAreReturned(knight, rank, file, expected);
    }

    [Theory]
    [InlineData(3, 3, 5, 4, "5,2|5,4|1,2|1,4|4,1|2,1|4,5|2,5")]
    [InlineData(7, 0, 5, 1, "5,1|6,2")]
    public void KnightIsNotBlockedByEnemyPieces(int rank, int file, int blockerRank, int blockerFile, string expected)
    {
        var knight = new Knight(Colour.White);

        Board[blockerRank, blockerFile] = new Pawn(Colour.Black);

        AssertAllExpectedMovesAreReturned(knight, rank, file, expected);
    }
}