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
}