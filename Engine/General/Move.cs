using System.Collections.Generic;
using Engine.Pieces;

namespace Engine.General
{
    public class Move
    {
        public Position FromPosition { get; set; }

        public Position ToPosition { get; set; }

        public int TotalValue { get; set; }

        public Board BoardState { get; set; }

        public List<Move> NextMoves { get; set; }

        public Move()
        {
            NextMoves = new List<Move>();
        }
    }
}