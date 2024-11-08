using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class King : Piece
    {
        public override Type Type => Type.King;

        public override int Value => 900;

        public override List<Position> PossibleMoves(Board board)
        {
            var builder = new PossibleMoveBuilder(board, Side);

            builder.AddMove(Position, -1, -1);
            builder.AddMove(Position, -1, 0);
            builder.AddMove(Position, -1, 1);
            builder.AddMove(Position, 0, 1);
            builder.AddMove(Position, 1, 1);
            builder.AddMove(Position, 1, 0);
            builder.AddMove(Position, 1, -1);
            builder.AddMove(Position, 0, -1);

            return builder.PossibleMoves;
        }

        public override Piece Copy()
        {
            return new King
                   {
                       NumberOfMoves = NumberOfMoves,
                       Position = Position.Copy(),
                       Side = Side
                   };
        }
    }
}