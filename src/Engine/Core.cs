using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private readonly Board _board;

    public Core(Board board)
    {
        _board = board;
    }

    public (int Combinations, int Move) GetMove(Kind side, int depth = 6)
    {
        for (var file = 0; file < 8; file++)
        {
            for (var rank = 0; rank < 8; rank++)
            {
                var kind = _board[file, rank];
                
                if (kind == Kind.Empty)
                {
                    continue;
                }

                if ((kind & side) == 0)
                {
                    continue;
                }

                Piece piece = (kind & Kind.TypeMask) switch
                {
                    Kind.Rook => new Rook(),
                    Kind.Knight => new Knight(),
                    Kind.Bishop => new Bishop(),
                    Kind.Queen => new Queen(),
                    Kind.King => new King(),
                    _ => new Pawn()
                };

                var moves = piece.GetMoves(file, rank, _board);

                foreach (var move in moves)
                {
                    var position = Board.GetRankAndFile(move);
                    
                    _board.Move(file, rank, position.File, position.Rank);
                }
            }
        }
        
        return (0, 0);
    }
}