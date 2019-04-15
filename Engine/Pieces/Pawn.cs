using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Pawn : Piece
    {
        public override Type Type => Type.Pawn;

        public override int Value => 1;

        public override List<Position> PossibleMoves(Board board)
        {
            var builder = new PossibleMoveBuilder(board, Side);

            if (NumberOfMoves == 0)
            {
                builder.AddMove(Position, 2, 0, true);
                builder.AddMove(Position, 1, 0, true);
            }
            else
            {
                builder.AddMove(Position, 1, 0, true);
            }

            builder.AddMove(Position, 1, 1, false, true);
            builder.AddMove(Position, 1, -1, false, true);

            return builder.PossibleMoves;
        }

        public override Piece Copy()
        {
            return new Pawn
                   {
                       NumberOfMoves = NumberOfMoves,
                       Position = new Position(Position.Row, Position.Column),
                       Side = Side
                   };
        }
    }
}