using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Queen : Piece
    {
        public override Type Type => Type.Queen;

        public override int Value => 9;

        public override List<Position> PossibleMoves(Board board)
        {
            var builder = new PossibleMoveBuilder(board, Side);

            builder.AddDirection(Position, -1, -1);
            builder.AddDirection(Position, -1, 0);
            builder.AddDirection(Position, -1, 1);
            builder.AddDirection(Position, 0, 1);
            builder.AddDirection(Position, 1, 1);
            builder.AddDirection(Position, 1, 0);
            builder.AddDirection(Position, 1, -1);
            builder.AddDirection(Position, 0, -1);

            return builder.PossibleMoves;
        }

        public override Piece Copy()
        {
            return new Queen
                   {
                       NumberOfMoves = NumberOfMoves,
                       Position = Position.Copy(),
                       Side = Side
                   };
        }
    }
}