using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private Board _board;

    private readonly Dictionary<int, int> _depthCounts = new();

    public IReadOnlyDictionary<int, int> DepthCounts => _depthCounts.Reverse().ToDictionary();
    
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
        
        GetMoveInternal(colour, depth);
    }

    private void GetMoveInternal(Colour colour, int depth)
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
                    _depthCounts[depth]++;
                    
                    _board.MakeMove(cell, move);

                    if (depth > 1)
                    {
                        GetMoveInternal(colour.Invert(), depth - 1);
                    }
                    
                    _board.UndoMove();
                }
            }
        }
    }
}