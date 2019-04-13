using Engine.General;
using Engine.Pieces;
using NUnit.Framework;

namespace Engine.Tests.General
{
    public class PossibleMoveBuilderTests
    {
        [TestCase(0, 0, -1, 0)]
        [TestCase(0, 0, 0, -1)]
        [TestCase(0, 7, 0, 1)]
        [TestCase(0, 7, -1, 0)]
        [TestCase(7, 0, 1, 0)]
        [TestCase(7, 0, 0, -1)]
        [TestCase(7, 7, 1, 0)]
        [TestCase(7, 7, 0, 1)]
        public void Does_not_add_move_if_out_of_bounds(int startRow, int startCol, int forwards, int right)
        {
            var builder = new PossibleMoveBuilder(new Board(), Side.Black);

            builder.AddMove(new Position(startRow, startCol), forwards, right);

            Assert.That(builder.PossibleMoves.Count, Is.Zero);
        }

        [Test]
        public void Does_not_add_space_must_be_empty()
        {
            var board = new Board
                        {
                            Squares = { [2, 2] = new Pawn() }
                        };

            var builder = new PossibleMoveBuilder(board, Side.Black);

            builder.AddMove(new Position(1, 2), 1, 0, true);

            Assert.That(builder.PossibleMoves.Count, Is.Zero);
        }
    }
}