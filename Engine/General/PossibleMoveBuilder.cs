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
            }

            if (_board.Squares[newPosition.Row, newPosition.Column] != null && _board.Squares[newPosition.Row, newPosition.Column].Side == _side)
            {
                return;
            }

            PossibleMoves.Add(newPosition);
        }

        public void AddDirection(Position currentPosition, int forward, int right)
        {
            if (_side == Side.White)
            {
                forward = -forward;
            }

            for (var i = 1; i < 8; i++)
            {
                var row = currentPosition.Row + forward * i;
                var column = currentPosition.Column + right * i;

                if (row < 0 || row > 7 || column < 0 || column > 7)
                {
                    break;
                }

                var target = _board.Squares[row, column];

                if (target == null)
                {
                    PossibleMoves.Add(new Position(row, column));
                    continue;
                }

                if (target.Side != _side)
                {
                    PossibleMoves.Add(new Position(row, column));
                }

                break;
            }
        }
    }
}