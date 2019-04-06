using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class King : Piece
    {
        public override Type Type => Type.King;

        public override List<Position> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}