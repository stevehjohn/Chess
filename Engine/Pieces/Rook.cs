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
            var builder = new PossibleMoveBuilder(board, Side);

            builder.AddDirection(Position, -1, 0);
            builder.AddDirection(Position, 0, -1);
            builder.AddDirection(Position, 1, 0);
            builder.AddDirection(Position, 0, 1);

            return builder.PossibleMoves;
        }

        public override Piece Copy()
        {
            return new Rook
                   {
                       NumberOfMoves = NumberOfMoves,
                       Position = Position.Copy(),
                       Side = Side
                   };
        }
    }
}