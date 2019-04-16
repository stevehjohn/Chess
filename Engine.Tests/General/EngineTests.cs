using Engine.General;
using Engine.Pieces;
using NCrunch.Framework;
using NUnit.Framework;
using System;
using System.Linq;

namespace Engine.Tests.General
{
    [TestFixture]
    public class EngineTests
    {
        private Board _board;
        private ChessEngine _engine;

        [SetUp]
        public void SetUp()
        {
            _board = BoardBuilder.Build();

            _engine = new ChessEngine(_board);
        }

        [Explicit]
        [Test]
        [Timeout(0)]
        public void Total_moves_at_each_depth_is_correct()
        {
            _engine.GetMove(Side.White);

            for (var i = 0; i < 4; i++)
            {
                var max = _engine.Depths[i].Max(m => m.TotalValue);
                var count = _engine.Depths[i].Count(m => m.TotalValue == max);

                Console.WriteLine( $"Max {i} {max}, {count}");
            }
        }
    }
}
