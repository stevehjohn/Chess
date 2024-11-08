using Engine.General;
using Engine.Pieces;
using NUnit.Framework;

namespace Engine.Tests.Pieces;

[TestFixture]
public class BishopTests
{
    private Board _board;
    private Bishop _bishop;

    [SetUp]
    public void SetUp()
    {
        _board = new Board();
        _bishop = new Bishop
        {
            Side = Side.White
        };
    }

    [Test]
    public void Can_move_diagonally_anywhere()
    {
        _bishop.Position = new Position(3, 3);

        var moves = _bishop.PossibleMoves(_board);

        Assert.That(moves.Count, Is.EqualTo(13));
    }

    [Test]
    public void Is_blocked_by_own_piece()
    {
        _board.Squares[2, 2] = new Pawn
        {
            Side = Side.White
        };
        _bishop.Position = new Position(3, 3);

        var moves = _bishop.PossibleMoves(_board);

        Assert.That(moves.Count, Is.EqualTo(10));
    }

    [Test]
    public void Stops_at_opponent()
    {
        _board.Squares[2, 2] = new Pawn
        {
            Side = Side.Black
        };
        _bishop.Position = new Position(3, 3);

        var moves = _bishop.PossibleMoves(_board);

        Assert.That(moves.Count, Is.EqualTo(11));
    }
}