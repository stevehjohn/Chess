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

        Assert.Equal("rn    nr|pppppppp|        |        |        |        |PPPPPPPP|RN    NR", _board.ToString());
    }

    [Fact]
    public void UndoMoveWorks()
    {
        _board.InitialisePieces();
        
        Assert.Equal("rn    nr|pppppppp|        |        |        |        |PPPPPPPP|RN    NR", _board.ToString());

        _board.MakeMove((Constants.WhitePawnRank, 0).GetCellIndex(), (5, 0).GetCellIndex());
        
        Assert.Equal("rn    nr|pppppppp|        |        |        |P       | PPPPPPP|RN    NR", _board.ToString());
        
        _board.UndoMove();
        
        Assert.Equal("rn    nr|pppppppp|        |        |        |        |PPPPPPPP|RN    NR", _board.ToString());
    }
}