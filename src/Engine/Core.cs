using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private Board _board;

    private readonly Dictionary<int, long> _depthCounts = new();

    private readonly Dictionary<string, long> _perftCounts = new();

    public IReadOnlyDictionary<int, long> DepthCounts => _depthCounts;

    public IReadOnlyDictionary<string, long> PerftCounts => _perftCounts;
    
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

        _perftCounts.Clear();

        GetMoveInternal(colour, depth, depth);
    }

    private void GetMoveInternal(Colour colour, int maxDepth, int depth, string perftNode = null)
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

                    if (perftNode == null)
                    {
                        perftNode = (rank, file).ToStandardNotation();
                    }
                    else if (perftNode.Length == 2)
                    {
                        perftNode = $"{perftNode}{(rank, file).ToStandardNotation()}";
                        
                        _perftCounts.TryAdd(perftNode, 0);
                    }
                    else
                    {
                        _perftCounts[perftNode]++;
                    }

                    _board.MakeMove(cell, move, ply);

                    if (depth > 1)
                    {
                        GetMoveInternal(colour.Invert(), maxDepth, depth - 1, perftNode);
                    }
                    
                    _board.UndoMove();
                }
            }
        }
    }
}