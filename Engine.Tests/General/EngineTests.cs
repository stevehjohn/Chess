using Engine.General;
using Engine.Pieces;
using NUnit.Framework;
using System.Threading.Tasks;

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
        public void Total_moves_at_each_depth_is_correct()
        {
            _engine.MakeMove(Side.White);
        }
    }
}
