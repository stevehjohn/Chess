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
    public void BoardPromotesWhitePawnToQueen()
    {
        _board.InitialisePieces("8/2P5/8/8/8/8/8/8");

        Assert.Equal("        |  P     |        |        |        |        |        |        ", _board.ToString());

        _board.MakeMove(10, 2, 1);
        
        Assert.Equal("  Q     |        |        |        |        |        |        |        ", _board.ToString());
    }
    
    [Fact]
    public void BoardPromotesBlackPawnToQueen()
    {
        _board.InitialisePieces("8/8/8/8/8/8/6p1/8");

        Assert.Equal("        |        |        |        |        |        |      p |        ", _board.ToString());

        _board.MakeMove(54, 62, 1);
        
        Assert.Equal("        |        |        |        |        |        |        |      q ", _board.ToString());
    }
}