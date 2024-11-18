using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private static readonly List<(int RankDelta, int FileDelta)> Orthogonals =
    [
        (-1, 0),
        (0, -1),
        (1, 0),
        (0, 1)
    ];

    private static readonly List<(int RankDelta, int FileDelta)> Diagonals =
    [
        (-1, -1),
        (1, -1),
        (-1, 1),
        (1, 1)
    ];

    private static readonly List<(int RankDelta, int FileDelta)> Knights =
    [
        (2, -1),
        (2, 1),
        (-2, -1),
        (-2, 1),
        (1, -2),
        (-1, -2),
        (1, 2),
        (-1, 2)
    ];

    private Board _board;

    private readonly Dictionary<int, long> _depthCounts = new();

    public IReadOnlyDictionary<int, long> DepthCounts => _depthCounts;
    
    public void Initialise()
    {
        _board = new Board();
        
        _board.InitialisePieces();
    }

    public void GetMove(Colour colour, int depth)
    {
        _depthCounts.Clear();

        for (var i = 1; i <= depth; i++)
        {
            _depthCounts[i] = 0;
        }
        
        GetMoveInternal(colour, depth, depth);
    }

    private void GetMoveInternal(Colour colour, int maxDepth, int depth)
    {
        for (var rank = 0; rank < Constants.Ranks; rank++)
        {
            for (var file = 0; file < Constants.Files; file++)
            {
                var cell = (rank, file).GetCellIndex();
                
                if (_board.IsEmpty(cell))
                {
                    continue;
                }

                if (! _board.IsColour(cell, colour))
                {
                    continue;
                }

                var piece = _board[rank, file];

                var moves = piece.GetMoves(rank, file, _board);

                foreach (var move in moves)
                {
                    var ply = maxDepth - depth + 1;
                    
                    _depthCounts[ply]++;
                    
                    _board.MakeMove(cell, move, ply);

                    if (IsKingInCheck(colour))
                    {
                        _board.UndoMove();
                        
                        _depthCounts[ply]--;
                    
                        continue;
                    }

                    if (depth > 1)
                    {
                        GetMoveInternal(colour.Invert(), maxDepth, depth - 1);
                    }
                    
                    _board.UndoMove();
                }
            }
        }
    }

    private bool IsKingInCheck(Colour colour)
    {
        (int Rank, int File) kingCell = (0, 0);

        int cell;
        
        for (var rank = 0; rank < Constants.Ranks; rank++)
        {
            for (var file = 0; file < Constants.Files; file++)
            {
                cell = (rank, file).GetCellIndex();
                
                if (_board.IsColour(cell, colour) && _board.CellKind(cell) == Kind.King)
                {
                    kingCell = (rank, file);

                    break;
                }
            }
        }

        (int Rank, int File) checkCell;

        foreach (var direction in Orthogonals)
        {
            checkCell = kingCell;
            
            for (var i = 0; i < Constants.MaxMoveDistance; i++)
            {
                checkCell.Rank += direction.RankDelta;

                checkCell.File += direction.FileDelta;

                cell = checkCell.GetCellIndex();

                if (cell < 0)
                {
                    break;
                }

                if (_board.IsColour(cell, colour))
                {
                    break;
                }

                var kind = _board.CellKind(cell);

                if (kind is Kind.Rook or Kind.Queen)
                {
                    return true;
                }

                if (kind is Kind.King && i == 0)
                {
                    return true;
                }
            }
        }

        foreach (var direction in Diagonals)
        {
            checkCell = kingCell;
            
            for (var i = 0; i < Constants.MaxMoveDistance; i++)
            {
                checkCell.Rank += direction.RankDelta;

                checkCell.File += direction.FileDelta;

                cell = checkCell.GetCellIndex();
        
                if (cell < 0)
                {
                    break;
                }
        
                if (_board.IsColour(cell, colour))
                {
                    break;
                }
        
                var kind = _board.CellKind(cell);
        
                if (kind is Kind.Bishop or Kind.Queen)
                {
                    return true;
                }
                
                if (kind is Kind.King && i == 0)
                {
                    return true;
                }
            }
        }

        foreach (var direction in Knights)
        {
            checkCell = kingCell;
            
            checkCell.Rank += direction.RankDelta;

            checkCell.File = direction.FileDelta;

            cell = checkCell.GetCellIndex();
        
            if (cell < 0)
            {
                break;
            }
        
            if (_board.IsColour(cell, colour))
            {
                continue;
            }
        
            var kind = _board.CellKind(cell);
        
            if (kind == Kind.Knight)
            {
                return true;
            }
        }
        
        // TODO: Pawn
        
        return false;
    }
}