using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public abstract class PieceTestBase
{
    protected readonly Board Board = new();
    
    protected void AssertAllExpectedMovesAreReturned(Piece piece, int rank, int file, string expected)
    {
        var moves = piece.GetMoves(rank, file, Board).ToList();

        if (expected == string.Empty)
        {
            Assert.Empty(moves);
            
            return;
        }

        var expectedMoves = expected.Split('|');
        
        Assert.Equal(expectedMoves.Length, moves.Count);
    }
}