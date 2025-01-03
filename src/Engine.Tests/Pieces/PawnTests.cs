using Engine.Extensions;
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
        var pawn = new Pawn(Colour.White);

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

    [Theory]
    [InlineData(Constants.WhitePawnRank, 0, "5,0")]
    [InlineData(Constants.BlackPawnRank, 0, "2,0")]
    public void PawnReturnsCorrectMovesHavingMoved(int rank, int file, string expected)
    {
        Board.InitialisePieces();

        var pawn = Board[rank, file];

        pawn.LastMovePly = 1;

        AssertAllExpectedMovesAreReturned(pawn, rank, file, expected);
    }

    [Theory]
    [InlineData(Constants.WhitePawnRank, 2, "5,1|5,2|4,2", 5, 1)]
    public void PawnMoveIncludeCaptureMove(int rank, int file, string expected, int blockerRank, int blockerFile)
    {
        Board.InitialisePieces();

        Board[blockerRank, blockerFile] = new Pawn(Colour.Black);

        var pawn = Board[rank, file];

        AssertAllExpectedMovesAreReturned(pawn, rank, file, expected);
    }
    
    [Theory]
    [InlineData(Constants.WhitePawnRank, 2, "5,1|5,2", 5, 1)]
    public void PawnMoveIncludeCaptureMoveAfterFirstTurn(int rank, int file, string expected, int blockerRank, int blockerFile)
    {
        Board.InitialisePieces();
        
        Board[blockerRank, blockerFile] = new Pawn(Colour.Black);
    
        var pawn = Board[rank, file];
    
        pawn.LastMovePly = 1;
    
        AssertAllExpectedMovesAreReturned(pawn, rank, file, expected);
    }
    
    [Theory]
    [InlineData(Constants.WhitePawnRank, 2, "5,2", 4, 2)]
    [InlineData(Constants.WhitePawnRank, 2, "", 5, 2)]
    public void PawnExcludesMoveIfBlocked(int rank, int file, string expected, int blockerRank, int blockerFile)
    {
        Board.InitialisePieces();
        
        Board[blockerRank, blockerFile] = new Pawn(Colour.Black);
    
        var pawn = Board[rank, file];
    
        pawn.LastMovePly = 1;
    
        AssertAllExpectedMovesAreReturned(pawn, rank, file, expected);
    }
    
    [Fact]
    public void PawnCanStillCaptureIfBlocked()
    {
        Board.InitialisePieces();

        Board[5, 0] = new Pawn(Colour.Black);

        Board[5, 1] = new Pawn(Colour.Black);
    
        var pawn = Board[Constants.WhitePawnRank, 0];
    
        pawn.LastMovePly = 1;
    
        var moves = pawn.GetMoves(Constants.WhitePawnRank, 0, 1, Board).ToList();
        
        Assert.Single(moves);
    
        Assert.Equal((5, 1).GetCellIndex(), moves.First());
    }
}