using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Rook : Piece
    {
        public override Type Type => Type.Rook;

        public override int Value => 5;

        public override List<Position> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}