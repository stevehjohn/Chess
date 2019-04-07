using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Knight : Piece
    {
        public override Type Type => Type.Knight;

        public override List<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}