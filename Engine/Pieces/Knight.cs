using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Knight : Piece
    {
        public override Type Type => Type.Knight;

        public override int Value => 30;

        public override List<Position> PossibleMoves(Board board)
        {
            var builder = new PossibleMoveBuilder(board, Side);

            builder.AddMove(Position, 2, 1);
            builder.AddMove(Position, 2, -1);
            builder.AddMove(Position, 1, 2);
            builder.AddMove(Position, 1, -2);
            builder.AddMove(Position, -1, 2);
            builder.AddMove(Position, -1, -2);
            builder.AddMove(Position, -2, 1);
            builder.AddMove(Position, -2, -1);

            return builder.PossibleMoves;
        }

        public override Piece Copy()
        {
            return new Knight
                   {
                       NumberOfMoves = NumberOfMoves,
                       Position = Position.Copy(),
                       Side = Side
                   };
        }
    }
}