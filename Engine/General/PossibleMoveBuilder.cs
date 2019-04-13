using Engine.Pieces;
using System.Collections.Generic;

namespace Engine.General
{
    public class PossibleMoveBuilder
    {
        private readonly Board _board;
        private readonly Side _side;

        public List<Position> PossibleMoves { get; }

        public PossibleMoveBuilder(Board board, Side side)
        {
            PossibleMoves = new List<Position>();
            _board = board;
            _side = side;
        }

        public void AddMove(Position currentPosition, int forward, int right, bool mustBeEmpty = false, bool mustContainOpponent = false)
        {
            if (_side == Side.White)
            {
                forward = -forward;
            }

            var newPosition = new Position(currentPosition.Row + forward, currentPosition.Column + right);

            if (newPosition.Row < 0 || newPosition.Row > 7 || newPosition.Column < 0 || newPosition.Column > 7)
            {
                return;
            }

            if (mustBeEmpty && _board.Squares[newPosition.Row, newPosition.Column] != null)
            {
                return;
            }

            if (mustContainOpponent)
            {
                if (_board.Squares[newPosition.Row, newPosition.Column] == null)
                {
                    return;
                }

                if (_board.Squares[newPosition.Row, newPosition.Column].Side == _side)
                {
                    return;
                }
            }

            PossibleMoves.Add(newPosition);
        }
    }
}