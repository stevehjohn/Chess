using Engine.General;
using Engine.Pieces;
using NUnit.Framework;

namespace Engine.Tests.Pieces
{
    [TestFixture]
    public class KingTests
    {
        private Board _board;
        private Piece _king;

        [SetUp]
        public void SetUp()
        {
            _board = new Board();
            _king = new King
                    {
                        Side = Side.White
                    };
        }

        [Test]
        public void Returns_correct_starting_moves()
        {
            _board = new Board
                     {
                         Squares =
                         {
                             [7, 3] = new Queen { Side = Side.White },
                             [7, 5] = new Bishop { Side = Side.White },
                             [6, 3] = new Pawn { Side = Side.White },
                             [6, 4] = new Pawn { Side = Side.White },
                             [6, 5] = new Pawn { Side = Side.White }
                         }
                     };

            _king.Position = new Position(7, 4);

            var moves = _king.PossibleMoves(_board);

            Assert.That(moves.Count, Is.Zero);
        }
    }
}