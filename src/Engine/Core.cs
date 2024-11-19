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
    
    private readonly Dictionary<(long Ply, PlyOutcome Outcome), int> _outcomes = new();

    public IReadOnlyDictionary<int, long> DepthCounts => _depthCounts;

    public IReadOnlyDictionary<(long Ply, PlyOutcome Outcome), int> Outcomes => _outcomes;
    
    public void Initialise()
    {
        _board = new Board();
        
        _board.InitialisePieces();
    }

    public void GetMove(Colour colour, int depth)
    {
        _depthCounts.Clear();
        
        _outcomes.Clear();

        for (var i = 1; i <= depth; i++)
        {
            _depthCounts[i] = 0;

            foreach (var outcome in Enum.GetValuesAsUnderlyingType<PlyOutcome>())
            {
                _outcomes[(i, (PlyOutcome) outcome)] = 0;
            }
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

                var ply = maxDepth - depth + 1;
                    
                var moves = piece.GetMoves(rank, file, ply, _board);

                foreach (var move in moves)
                {
                    _depthCounts[ply]++;
                    
                    var outcome = _board.MakeMove(cell, move, ply);

                    if (IsKingInCheck(colour))
                    {
                        _board.UndoMove();
                        
                        _depthCounts[ply]--;
                    
                        continue;
                    }

                    if (IsKingInCheck(colour.Invert()))
                    {
                        outcome = PlyOutcome.Check;
                    }

                    _outcomes[(ply, outcome)]++;

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
        var kingCellIndex = colour == Colour.Black ? _board.BlackKingCell : _board.WhiteKingCell;

        var kingCell = (Rank: kingCellIndex / 8, File: kingCellIndex % 8);

        int cell;

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

                if (! _board.IsEmpty(cell))
                {
                    break;
                }
            }
        }

        foreach (var direction in Knights)
        {
            checkCell = kingCell;
            
            checkCell.Rank += direction.RankDelta;

            checkCell.File += direction.FileDelta;

            cell = checkCell.GetCellIndex();
        
            if (cell < 0)
            {
                continue;
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

        var rankDirection = colour == Colour.Black ? 1 : -1;

        cell = (kingCell.Rank + rankDirection, kingCell.File - 1).GetCellIndex();

        if (cell >= 0 && _board.IsColour(cell, colour.Invert()) && _board.CellKind(cell) == Kind.Pawn)
        {
            return true;
        }

        cell = (kingCell.Rank + rankDirection, kingCell.File + 1).GetCellIndex();

        if (cell >= 0 && _board.IsColour(cell, colour.Invert()) && _board.CellKind(cell) == Kind.Pawn)
        {
            return true;
        }
        
        return false;
    }
}