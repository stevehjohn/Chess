using Engine.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace Engine.General
{
    public class ChessEngine
    {
        private const int Depth = 4;

        private readonly Board _board;

        internal List<Move>[] Depths;

        public ChessEngine(Board board)
        {
            _board = board;
        }

        public void MakeMove(Side side)
        {
            Depths = new List<Move>[Depth];

            for (var depth = 0; depth < Depth; depth++)
            {
                Depths[depth] = new List<Move>();
            }

            GetMoves(side, _board);
        }

        private void GetMoves(Side side, Board board, int depth = 0, int previousValue = 0)
        {
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    var piece = board.Squares[row, column];

                    if (piece != null)
                    {
                        if (piece.Side == side)
                        {
                            var pieceMoves = piece.PossibleMoves(board).ToList();

                            foreach (var move in pieceMoves)
                            {
                                var value = 0;

                                var boardCopy = board.Copy();
                                var target = boardCopy.Squares[move.Row, move.Column];
                                if (target != null)
                                {
                                    value = target.Value;
                                }
                                boardCopy.Squares[piece.Position.Row, piece.Position.Column] = null;

                                boardCopy.Squares[move.Row, move.Column] = piece.Copy();

                                var totalValue = previousValue + value;

                                Depths[depth].Add(new Move
                                {
                                    FromPosition = piece.Position.Copy(),
                                    ToPosition = move,
                                    TotalValue = totalValue
                                });

                                if (depth < Depth - 1)
                                {
                                    GetMoves((Side)(-(int)side), boardCopy, depth + 1, totalValue);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}