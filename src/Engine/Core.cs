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

    public void GetMove(Kind side, int depth = 6)
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

                var piece = kind switch
                {
                    _ => new Pawn()
                };

                var moves = piece.GetMoves(file, rank, _board);

                foreach (var move in moves)
                {
                    
                }
            }
        }
    }
}