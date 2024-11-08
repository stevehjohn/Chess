using Engine.General;
using NUnit.Framework;
// ReSharper disable StringLiteralTypo

namespace Engine.Tests.General;

[TestFixture]
public class BoardBuilderTests
{
    private static readonly string[] Expected =
    [
        "♜♞♝♛♚♝♞♜",
        "♟♟♟♟♟♟♟♟",
        "        ",
        "        ",
        "        ",
        "        ",
        "♙♙♙♙♙♙♙♙",
        "♖♘♗♕♔♗♘♖"
    ];

    [Test]
    public void Builds_initial_board_correctly()
    {
        var board = BoardBuilder.Build();

        Assert.That(board.Dump(), Is.EqualTo(Expected));
    }
}