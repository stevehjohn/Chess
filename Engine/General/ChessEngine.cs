using Engine.Pieces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task MakeMove(Side side)
        {
            Depths = new List<Move>[Depth];

            for (var depth = 0; depth < Depth; depth++)
            {
                Depths[depth] = new List<Move>();
            }

            GetMoves(side, _board);
        }

        private void GetMoves(Side side, Board board, int depth = 0)
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

                            if (depth < Depth - 1)
                            {
                                foreach (var move in pieceMoves)
                                {
                                    var boardCopy = board.Copy();
                                    boardCopy.Squares[piece.Position.Row, piece.Position.Column] = null;
                                    boardCopy.Squares[move.Row, move.Column] = piece.Copy();

                                    GetMoves((Side) (-(int) side), boardCopy, depth + 1);
                                }
                            }

                            Depths[depth].AddRange(pieceMoves.Select(p => new Move
                                                                          {
                                                                              FromPosition = piece.Position.Copy(),
                                                                              ToPosition = p
                                                                          }));
                        }
                    }
                }
            }
        }
    }
}