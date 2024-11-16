using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.General;

public class PieceTests
{
    private readonly Board _board = new();
    
    [Fact]
    public void InitialiseSetsUpBoardCorrectly()
    {
        _board.Initialise();

        for (var file = 0; file < Constants.Files; file++)
        {
            var piece = _board[Constants.BlackPawnRank, file];
            
            Assert.Equal(Kind.Pawn, piece.Kind);
            Assert.Equal(Colour.Black, piece.Colour);

            piece = _board[Constants.WhitePawnRank, file];
            
            Assert.Equal(Kind.Pawn, piece.Kind);
            Assert.Equal(Colour.White, piece.Colour);
        }
    }
}