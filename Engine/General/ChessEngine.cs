using Engine.Pieces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Engine.Helpers;

namespace Engine.General
{
    public class ChessEngine
    {
        internal int Depth;
        private readonly bool _concurrent;

        private readonly Board _board;

        private ConcurrentBag<Task> _tasks;

        internal ConcurrentBag<Move>[] Depths;

        public ChessEngine(Board board, int depth, bool concurrent)
        {
            _board = board;
            Depth = depth;
            _concurrent = concurrent;
        }

        public Move GetMove(Side side)
        {
            Depths = new ConcurrentBag<Move>[Depth];
            _tasks = new ConcurrentBag<Task>();

            for (var depth = 0; depth < Depth; depth++)
            {
                Depths[depth] = new ConcurrentBag<Move>();
            }

            GetMoves(side, _board);

            if (_concurrent)
            {
                TaskHelpers.WaitForAllToComplete(_tasks.ToArray());
            }

            var bestScore = Depths[Depth - 1].Max(m => m.TotalValue);

            var moves = Depths[Depth - 1].Where(m => m.TotalValue == bestScore).ToList();

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

                        if (depth >= Depth - 1)
                        {
                            continue;
                        }

                        if (_concurrent)
                        {
                            _tasks.Add(Task.Run(() => GetMoves((Side) (-(int) side), boardCopy, depth + 1, totalValue, move)));
                        }
                        else
                        {
                            GetMoves((Side) (-(int) side), boardCopy, depth + 1, totalValue, move);
                        }
                    }
                }
            }
        }
    }
}