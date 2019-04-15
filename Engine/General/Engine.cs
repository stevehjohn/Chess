using Engine.Pieces;
using System.Diagnostics;

namespace Engine.General
{
    public class Engine
    {
        private readonly Board _board;

        public Engine(Board board)
        {
            _board = board;
        }

        public void MakeMove(Side side, int millisecondsToThink)
        {
            var stopWatch = Stopwatch.StartNew();

            while (stopWatch.ElapsedMilliseconds < millisecondsToThink)
            {

            }
        }
    }
}