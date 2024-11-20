using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
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
                    
                    if (_board.IsKingInCheck(colour, colour == Colour.Black ? _board.BlackKingCell : _board.WhiteKingCell))
                    {
                        _board.UndoMove();
                        
                        _depthCounts[ply]--;
                    
                        continue;
                    }

                    if (_board.IsKingInCheck(colour.Invert(), colour == Colour.White ? _board.BlackKingCell : _board.WhiteKingCell))
                    {
                        outcome = PlyOutcome.Check;
                    }

                    _outcomes[(ply, outcome)]++;

                    if (outcome == PlyOutcome.EnPassant)
                    {
                        _outcomes[(ply, PlyOutcome.Capture)]++;
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
}