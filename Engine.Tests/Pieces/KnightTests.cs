using Engine.General;
using Engine.Pieces;
using NUnit.Framework;
using System.Linq;

namespace Engine.Tests.Pieces;

[TestFixture]
public class KnightTests
{
    private Board _board;
    private Knight _knight;

    [SetUp]
    public void SetUp()
    {
        _board = new Board();
        _knight = new Knight
        {
            Side = Side.White
        };
    }

    [Test]
    public void Returns_correct_starting_moves()
    {
        _board.Squares[1, 3] = new Pawn
        {
            Side = Side.White
        };
        _knight.Position = new Position(0, 1);

        var moves = _knight.PossibleMoves(_board);

        Assert.That(moves.Count, Is.EqualTo(2));
        Assert.True(moves.Any(m => m.Row == 2 && m.Column == 0));
        Assert.True(moves.Any(m => m.Row == 2 && m.Column == 2));
    }
}