using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Queen : Piece
    {
        public override Type Type => Type.Queen;

        public override List<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}