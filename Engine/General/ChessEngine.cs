using Engine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.General
{
    public class ChessEngine
    {
        private readonly Board _board;
        private readonly int _depth;

        internal List<Move>[] Depths;

        public ChessEngine(Board board, int depth)
        {
            _board = board;
            _depth = depth;
        }

        public Move GetMove(Side side)
        {
            Depths = new List<Move>[_depth];

            for (var depth = 0; depth < _depth; depth++)
            {
                Depths[depth] = new List<Move>();
            }

            GetMoves(side, _board);

            var bestScore = Depths[_depth - 1].Max(m => m.TotalValue);

            var moves = Depths[_depth - 1].Where(m => m.TotalValue == bestScore).ToList();

            var random = new Random();

            var move = moves[random.Next(moves.Count)];

            while (move.PreviousMove != null)
            {
                move = move.PreviousMove;
            }

            return move;
        }

        private void GetMoves(Side side, Board board, int depth = 0, int previousValue = 0, Move previousMove = null)
        {
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    var piece = board.Squares[row, column];

                    if (piece == null)
                    {
                        continue;
                    }

                    if (piece.Side != side)
                    {
                        continue;
                    }

                    var pieceMoves = piece.PossibleMoves(board).ToList();

                    foreach (var position in pieceMoves)
                    {
                        var value = 0;

                        var boardCopy = board.Copy();
                        var target = boardCopy.Squares[position.Row, position.Column];
                        if (target != null)
                        {
                            value = target.Value;
                        }

                        boardCopy.Squares[piece.Position.Row, piece.Position.Column] = null;

                        boardCopy.Squares[position.Row, position.Column] = piece.Copy();

                        var totalValue = previousValue + value;

                        var move = new Move
                                   {
                                       FromPosition = piece.Position.Copy(),
                                       ToPosition = position,
                                       TotalValue = totalValue,
                                       PreviousMove = previousMove
                                   };

                        Depths[depth].Add(move);

                        if (depth >= _depth - 1)
                        {
                            continue;
                        }

                        GetMoves((Side) (-(int) side), boardCopy, depth + 1, totalValue, move);
                    }
                }
            }
        }
    }
}