using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private Board _board;
    
    private int _ply;

    private Colour _player;
    
    private readonly Dictionary<int, long> _depthCounts = new();
    
    private readonly Dictionary<(long Ply, PlyOutcome Outcome), long> _outcomes = new();

    private readonly Dictionary<string, long> _perftCounts = new();

    private readonly Dictionary<int, int> _plyBestScores = new();
    
    public IReadOnlyDictionary<int, long> DepthCounts => _depthCounts;

    public IReadOnlyDictionary<(long Ply, PlyOutcome Outcome), long> Outcomes => _outcomes;
        
    public IReadOnlyDictionary<string, long> PerftCounts => _perftCounts;
    
    public void Initialise(Colour colour = Colour.White)
    {
        _board = new Board();
        
        _ply = 1;

        _player = colour;
        
        _board.InitialisePieces();
    }
    
    public void Initialise(string fen)
    {
        _board = new Board();

        _ply = 1;
        
        var parts = fen.Split(' ');

        _player = parts[1] == "w" ? Colour.White : Colour.Black;
        
        _board.InitialisePieces(parts[0]);
    }
    
    public void MakeMove(int position, int target)
    {
        _board.MakeMove(position, target, _ply);
        
        _player = _player.Invert();

        _ply++;
    }
    
    public void GetMove(int depth)
    {
        _depthCounts.Clear();
        
        _outcomes.Clear();
        
        _perftCounts.Clear();
        
        _plyBestScores.Clear();
        
        for (var i = 1; i <= depth; i++)
        {
            _depthCounts[i] = 0;

            _plyBestScores[i] = 0;

            foreach (var outcome in Enum.GetValuesAsUnderlyingType<PlyOutcome>())
            {
                _outcomes[(i, (PlyOutcome) outcome)] = 0;
            }
        }
        
        GetMoveInternal(_board, _player, depth, depth);
    }

    private void GetMoveInternal(Board board, Colour colour, int maxDepth, int depth, string perftNode = null)
    {
        var isKingInCheck = board.IsKingInCheck(colour, colour == Colour.Black ? board.BlackKingCell : board.WhiteKingCell);

        var moved = false;
       
        var ply = maxDepth - depth + 1;
 
        for (var rank = 0; rank < Constants.Ranks; rank++)
        {
            for (var file = 0; file < Constants.Files; file++)
            {
                var cell = (rank, file).GetCellIndex();
                
                if (board.IsEmpty(cell))
                {
                    continue;
                }

                if (! board.IsColour(cell, colour))
                {
                    continue;
                }

                var piece = board[rank, file];
                    
                var moves = piece.GetMoves(rank, file, ply, board);

                foreach (var move in moves)
                {
                    _depthCounts[ply]++;

                    var copy = new Board(board);
                    
                    var outcome = copy.MakeMove(cell, move, ply);

                    var score = colour == Colour.Black ? copy.BlackScore : copy.WhiteScore;

                    if (copy.IsKingInCheck(colour, colour == Colour.Black ? copy.BlackKingCell : copy.WhiteKingCell))
                    {
                        _depthCounts[ply]--;
                    
                        continue;
                    }
                    
                    moved = true;
                    
                    // TODO: When not doing a full explore, stop if score is worse?
                    
                    if (score > _plyBestScores[ply])
                    {
                        _plyBestScores[ply] = score;
                    }
                    
                    if (perftNode == null)
                    {
                        perftNode = $"{(rank, file).ToStandardNotation()}{(move / 8, move % 8).ToStandardNotation()}";

                        _perftCounts.TryAdd(perftNode, 1);
                    }
                    else
                    {
                        _perftCounts[perftNode]++;
                    }

                    if (copy.IsKingInCheck(colour.Invert(), colour == Colour.White ? copy.BlackKingCell : copy.WhiteKingCell))
                    {
                        if (outcome == PlyOutcome.Capture)
                        {
                            _outcomes[(ply, PlyOutcome.Capture)]++;
                        }

                        if (outcome == PlyOutcome.EnPassant)
                        {
                            _outcomes[(ply, PlyOutcome.Capture)]++;

                            _outcomes[(ply, PlyOutcome.EnPassant)]++;
                        }

                        outcome = PlyOutcome.Check;
                    }

                    _outcomes[(ply, outcome)]++;

                    if (outcome == PlyOutcome.EnPassant)
                    {
                        _outcomes[(ply, PlyOutcome.Capture)]++;
                    }

                    if (depth > 1)
                    {
                        GetMoveInternal(copy, colour.Invert(), maxDepth, depth - 1, perftNode);

                        _perftCounts[perftNode]--;
                    }
                    
                    if (depth == maxDepth)
                    {
                        perftNode = null;
                    }
                }
            }
        }

        if (isKingInCheck && ! moved)
        {
            _outcomes[(ply, PlyOutcome.CheckMate)]++;
        }
    }
}