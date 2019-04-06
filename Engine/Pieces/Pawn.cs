using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Pawn : Piece
    {
        public override Type Type => Type.Pawn;

        public override List<Position> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}