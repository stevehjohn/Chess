using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Bishop : Piece
    {
        public override Type Type => Type.Bishop;

        public override List<Position> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}