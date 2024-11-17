using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class PawnTests : PieceTestBase
{
    [Theory]
    [InlineData(7, 0, "6,0|5,0")]
    public void PawnReturnsCorrectMovesOnEmptyBoard(int rank, int file, string expected)
    {
        var pawn = new Knight(Colour.White);

        AssertAllExpectedMovesAreReturned(pawn, rank, file, expected);
    }

    [Theory]
    [InlineData(Constants.WhitePawnRank, 0, "5,0|4,0")]
    [InlineData(Constants.BlackPawnRank, 0, "2,0|3,0")]
    public void PawnReturnsCorrectStartingMoves(int rank, int file, string expected)
    {
        Board.InitialisePieces();

        var pawn = Board[rank, file];

        AssertAllExpectedMovesAreReturned(pawn, rank, file, expected);
    }
}