using Engine.General;
using Engine.Pieces;
using NUnit.Framework;

namespace Engine.Tests.Pieces
{
    [TestFixture]
    public class RookTests
    {
        private Board _board;
        private Piece _rook;

        [SetUp]
        public void SetUp()
        {
            _board = new Board();
            _rook = new Rook
                    {
                        Side = Side.White
                    };
        }

        [Test]
        public void Can_move_horizontally_or_vertically_anywhere()
        {
            _rook.Position = new Position(3, 3);

            var moves = _rook.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(14));
        }

        [Test]
        public void Is_blocked_by_own_piece()
        {
            _board.Squares[3, 2] = new Pawn
                                   {
                                       Side = Side.White
                                   };
            _rook.Position = new Position(3, 3);

            var moves = _rook.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(11));
        }

        [Test]
        public void Stops_at_opponent()
        {
            _board.Squares[3, 2] = new Pawn
                                   {
                                       Side = Side.Black
                                   };
            _rook.Position = new Position(3, 3);

            var moves = _rook.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(12));
        }
    }
}