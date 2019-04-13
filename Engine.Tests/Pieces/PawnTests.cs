using Engine.General;
using Engine.Pieces;
using NUnit.Framework;
using System.Linq;

namespace Engine.Tests.Pieces
{
    [TestFixture]
    public class PawnTests
    {
        private Board _board;
        private Piece _pawn;

        [SetUp]
        public void SetUp()
        {
            _board = new Board();
            _pawn = new Pawn();
        }

        [Test]
        public void Returns_correct_starting_moves()
        {
            _pawn.Position = new Position(1, 1);

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(2));
            Assert.True(moves.Any(m => m.Row == 2 && m.Column == 1));
            Assert.True(moves.Any(m => m.Row == 3 && m.Column == 1));
        }

        [Test]
        public void Returns_correct_non_starting_moves_with_no_pieces_to_take()
        {
            _pawn.Position = new Position(2, 1);
            _pawn.NumberOfMoves = 1;

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(1));
            Assert.True(moves.Any(m => m.Row == 3 && m.Column == 1));
        }
    }
}