using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class KingTests : PieceTestBase
{
    [Theory]
    [InlineData(0, 0, "1,0|0,1|1,1")]
    [InlineData(Constants.BottomRank, Constants.RightmostFile, "6,7|7,6|6,6")]
    [InlineData(Constants.BottomRank, 0, "7,1|6,0|6,1")]
    [InlineData(0, Constants.RightmostFile, "0,6|1,7|1,6")]
    [InlineData(3, 3, "2,3|4,3|3,2|3,4|2,2|4,4|4,2|2,4")]
    public void KingReturnsCorrectMovesOnEmptyBoard(int rank, int file, string expected)
    {
        var king = new King(Colour.White);

        AssertAllExpectedMovesAreReturned(king, rank, file, expected);
    }
    
    [Theory]
    [InlineData(0, 0, 1, 0, "0,1|1,1")]
    [InlineData(Constants.BottomRank, Constants.RightmostFile, 7, 6, "6,7|6,6")]
    [InlineData(Constants.BottomRank, 0, 7, 1, "6,0|6,1")]
    [InlineData(0, Constants.RightmostFile, 0, 6, "1,7|1,6")]
    [InlineData(3, 3, 4, 3, "2,3|3,2|3,4|2,2|4,4|4,2|2,4")]
    public void KingIsBlockedByOwnPieces(int rank, int file, int blockerRank, int blockerFile, string expected)
    {
        var king = new King(Colour.White);

        Board[blockerRank, blockerFile] = new Pawn(Colour.White);
        
        AssertAllExpectedMovesAreReturned(king, rank, file, expected);
    }
    
    [Theory]
    [InlineData(0, 0, 1, 0, "1,0|0,1|1,1")]
    [InlineData(Constants.BottomRank, 7, 6, Constants.RightmostFile, "6,7|7,6|6,6")]
    [InlineData(Constants.BottomRank, 0, 7, 6, "7,1|6,0|6,1")]
    [InlineData(0, Constants.RightmostFile, 1, 7, "0,6|1,7|1,6")]
    [InlineData(3, 3, 3, 2, "2,3|4,3|3,2|3,4|2,2|4,4|4,2|2,4")]
    public void KingTakesAndStopsOnEnemyPiece(int rank, int file, int blockerRank, int blockerFile, string expected)
    {
        var king = new King(Colour.White);

        Board[blockerRank, blockerFile] = new Pawn(Colour.Black);
        
        AssertAllExpectedMovesAreReturned(king, rank, file, expected);
    }

    [Fact]
    public void KingCanCastleKingSide()
    {
        var king = new King(Colour.White);

        Board[Constants.WhiteHomeRank, Constants.KingFile] = king;

        Board[Constants.WhiteHomeRank, Constants.RightRookFile] = new Rook(Colour.White);

        var moves = king.GetMoves(Constants.WhiteHomeRank, Constants.KingFile, 1, Board);

        Assert.Contains(SpecialMoveCodes.CastleKingSide, moves);
    }

    [Fact]
    public void KingCanCastleQueenSide()
    {
        var king = new King(Colour.White);

        Board[Constants.WhiteHomeRank, Constants.KingFile] = king;

        Board[Constants.WhiteHomeRank, Constants.LeftRookFile] = new Rook(Colour.White);

        var moves = king.GetMoves(Constants.WhiteHomeRank, Constants.KingFile, 1, Board);

        Assert.Contains(SpecialMoveCodes.CastleQueenSide, moves);
    }
}