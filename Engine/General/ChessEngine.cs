using Engine.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace Engine.General
{
    public class ChessEngine
    {
        private const int Depth = 4;

        private readonly Board _board;

        public ChessEngine(Board board)
        {
            _board = board;
        }

        public void MakeMove(Side side)
        {
            var moves = new List<Move>();

            moves = GetMoves(side, _board, Depth);
        }

        private static List<Move> GetMoves(Side side, Board board, int depth)
        {
            depth--;

            var moves = new List<Move>();

            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    var piece = board.Squares[row, column];

                    if (piece != null)
                    {
                        if (piece.Side == side)
                        {
                            var pieceMoves = piece.PossibleMoves(board)
                                                  .Select(p =>
                                                  {
                                                      var boardCopy = board.Copy();
                                                      boardCopy.Squares[piece.Position.Row, piece.Position.Column] = null;
                                                      boardCopy.Squares[p.Row, p.Column] = piece.Copy();

                                                      return new Move
                                                             {
                                                                 BoardState = boardCopy,
                                                                 FromPosition = piece.Position.Copy(),
                                                                 ToPosition = p
                                                             };
                                                  })
                                                  .ToList();

                            moves.AddRange(pieceMoves);

                            if (depth > 0)
                            {
                                foreach (var move in pieceMoves)
                                {
                                    move.NextMoves.AddRange(GetMoves((Side)(-(int)side), move.BoardState, depth));
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }
    }
}