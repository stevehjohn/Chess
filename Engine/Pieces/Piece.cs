using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public abstract class Piece
    {
        public Side Side { get; set; }

        public abstract Type Type { get; }

        public int NumberOfMoves { get; set; }

        public Position Position { get; set; }

        public abstract List<Move> PossibleMoves(Board board);

        public bool Captured { get; set; }

        protected Piece()
        {
            NumberOfMoves = 0;
        }
    }
}