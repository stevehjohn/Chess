using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private readonly Board _board;

    private readonly Dictionary<int, int> _depthCombinations = [];
    
    public Core(Board board)
    {
        _board = board;
    }

    public (int Combinations, int Move) GetMove(Kind side, int depth = 6)
    {
        _depthCombinations.Clear();

        for (var i = 1; i <= depth; i++)
        {
            _depthCombinations[i] = 0;
        }

        var move = GetMoveInternal(side, depth);

        return (_depthCombinations[depth], move);
    }

    private int GetMoveInternal(Kind side, int depth)
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

                    _depthCombinations[depth]++;

                    if (depth > 1)
                    {
                        GetMoveInternal(side ^ Kind.ColourMask, depth - 1);
                    }
                    
                    _board.UndoMove();
                }
            }
        }
        
        return 0;
    }
}