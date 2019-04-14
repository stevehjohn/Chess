using Engine.General;
using Engine.Pieces;
using NUnit.Framework;

namespace Engine.Tests.Pieces
{
    [TestFixture]
    public class QueenTests
    {
        private Board _board;
        private Piece _queen;

        [SetUp]
        public void SetUp()
        {
            _board = new Board();
            _queen = new Queen
                     {
                         Side = Side.White
                     };
        }

        [Test]
        public void Can_move_where_she_likes()
        {
            _queen.Position = new Position(3, 3);

            var moves = _queen.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(27));
        }

        [Test]
        public void Is_blocked_by_own_piece()
        {
            _board.Squares[3, 1] = new Pawn
                                   {
                                       Side = Side.White
                                   };
            _queen.Position = new Position(3, 3);

            var moves = _queen.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(25));
        }

        [Test]
        public void Stops_at_opponent()
        {
            _board.Squares[3, 1] = new Pawn
                                   {
                                       Side = Side.Black
                                   };
            _queen.Position = new Position(3, 3);

            var moves = _queen.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(26));
        }
    }
}