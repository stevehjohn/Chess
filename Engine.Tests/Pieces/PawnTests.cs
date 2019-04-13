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
            _pawn = new Pawn
                    {
                        Side = Side.White
                    };
        }

        [Test]
        public void Returns_correct_starting_moves()
        {
            _pawn.Position = new Position(6, 1);

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(2));
            Assert.True(moves.Any(m => m.Row == 5 && m.Column == 1));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 1));
        }

        [Test]
        public void Returns_correct_non_starting_moves_with_no_pieces_to_take()
        {
            _pawn.Position = new Position(7, 1);
            _pawn.NumberOfMoves = 1;

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(1));
            Assert.True(moves.Any(m => m.Row == 6 && m.Column == 1));
        }

        [Test]
        public void Returns_correct_non_starting_moves_with_piece_to_take_1()
        {
            _board.Squares[4, 0] = new Pawn
                                   {
                                       Side = Side.Black
                                   };
            _pawn.Position = new Position(5, 1);
            _pawn.NumberOfMoves = 1;

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(2));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 1));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 0));
        }

        [Test]
        public void Returns_correct_non_starting_moves_with_piece_to_take_2()
        {
            _board.Squares[4, 2] = new Pawn
                                   {
                                       Side = Side.Black
                                   };
            _pawn.Position = new Position(5, 1);
            _pawn.NumberOfMoves = 1;

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(2));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 1));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 2));
        }

        [Test]
        public void Returns_correct_non_starting_moves_with_piece_to_take_3()
        {
            _board.Squares[4, 0] = new Pawn
                                   {
                                       Side = Side.Black
                                   };
            _board.Squares[4, 2] = new Pawn
                                   {
                                       Side = Side.Black
                                   };
            _pawn.Position = new Position(5, 1);
            _pawn.NumberOfMoves = 1;

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(3));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 1));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 0));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 2));
        }

        [Test]
        public void Returns_correct_non_starting_moves_with_friendlies_in_capturable_positions()
        {
            _board.Squares[4, 0] = new Pawn
                                   {
                                       Side = Side.White
                                   };
            _board.Squares[4, 2] = new Pawn
                                   {
                                       Side = Side.White
                                   };
            _pawn.Position = new Position(5, 1);
            _pawn.NumberOfMoves = 1;

            var moves = _pawn.PossibleMoves(_board);

            Assert.That(moves.Count, Is.EqualTo(1));
            Assert.True(moves.Any(m => m.Row == 4 && m.Column == 1));
        }
    }
}