using Engine.Extensions;
using Engine.General;
using Xunit;

namespace Engine.Tests.General;

public class BoardTests
{
    private readonly Board _board = new();
    
    [Fact]
    public void InitialisePiecesSetsUpBoardCorrectly()
    {
        _board.InitialisePieces();

        Assert.Equal("rnbqkbnr|pppppppp|        |        |        |        |PPPPPPPP|RNBQKBNR", _board.ToString());
    }

    [Fact]
    public void UndoMoveWorks()
    {
        _board.InitialisePieces();
        
        Assert.Equal("rnbqkbnr|pppppppp|        |        |        |        |PPPPPPPP|RNBQKBNR", _board.ToString());

        _board.MakeMove((Constants.WhitePawnRank, 0).GetCellIndex(), (5, 0).GetCellIndex(), 1);
        
        Assert.Equal("rnbqkbnr|pppppppp|        |        |        |P       | PPPPPPP|RNBQKBNR", _board.ToString());
        
        _board.UndoMove();
        
        Assert.Equal("rnbqkbnr|pppppppp|        |        |        |        |PPPPPPPP|RNBQKBNR", _board.ToString());
    }
}