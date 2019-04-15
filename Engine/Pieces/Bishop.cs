using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces
{
    public class Bishop : Piece
    {
        public override Type Type => Type.Bishop;

        public override int Value => 3;

        public override List<Position> PossibleMoves(Board board)
        {
            var builder = new PossibleMoveBuilder(board, Side);

            builder.AddDirection(Position, -1, -1);
            builder.AddDirection(Position, -1, 1);
            builder.AddDirection(Position, 1, 1);
            builder.AddDirection(Position, 1, -1);

            return builder.PossibleMoves;
        }

        public override Piece Copy()
        {
            return new Bishop
                   {
                       NumberOfMoves = NumberOfMoves,
                       Position = new Position(Position.Row, Position.Column),
                       Side = Side
                   };
        }
    }
}