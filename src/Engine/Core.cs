using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private Board _board;
    
    private int _ply;

    private long[] _depthCounts;

    private int[] _plyBestScores;

    private long[][] _outcomes;

    private readonly Dictionary<string, long> _perftCounts = new();

    private string _bestPath;

    public int MoveCount => _ply - 1;

    public long GetDepthCount(int ply) => _depthCounts[ply];

    public long GetPlyOutcome(int ply, PlyOutcome outcome) => _outcomes[ply][(int) outcome];
        
    public IReadOnlyDictionary<string, long> PerftCounts => _perftCounts;

    public Colour Player { get; private set; }

    public void Initialise(Colour colour = Colour.White)
    {
        _board = new Board();
        
        _ply = 1;

        Player = colour;
        
        _board.InitialisePieces();
    }
    
    public void Initialise(string fen)
    {
        _board = new Board();

        _ply = 1;
        
        var parts = fen.Split(' ');

        Player = parts[1] == "w" ? Colour.White : Colour.Black;
        
        _board.InitialisePieces(parts[0]);
    }

    public void MakeMove(string move)
    {
        MakeMove(move[..2].CellFromStandardNotation(), move[2..].CellFromStandardNotation());
    }

    public void MakeMove(int position, int target)
    {
        _board.MakeMove(position, target, _ply);
        
        Player = Player.Invert();

        _ply++;
    }
    
    public string GetMove(int depth)
    {
        _depthCounts = new long[depth + 1];
        
        _plyBestScores = new int[depth + 1];

        _outcomes = new long[depth + 1][];
        
        _perftCounts.Clear();
        
        for (var i = 1; i <= depth; i++)
        {
            _depthCounts[i] = 0;

            _plyBestScores[i] = 0;

            var outcomes = Enum.GetValuesAsUnderlyingType<PlyOutcome>();

            _outcomes[i] = new long[outcomes.Length];
        }

        if (GetMoveInternal(_board, Player, depth, depth, string.Empty))
        {
            return _bestPath[..4];
        }

        return null;
    }

    private bool GetMoveInternal(Board board, Colour colour, int maxDepth, int depth, string path, string perftNode = null)
    {
        var moved = false;
       
        var ply = maxDepth - depth + 1;
 
        for (var cell = 0; cell < Constants.BoardCells; cell++)
        {
            if (board.IsEmpty(cell))
            {
                continue;
            }

            if (! board.IsColour(cell, colour))
            {
                continue;
            }

            var rank = cell >> 3;

            var file = cell & 7;
            
            var piece = board[rank, file];
                
            var moves = piece.GetMoves(rank, file, ply, board);

            foreach (var move in moves)
            {
                _depthCounts[ply]++;

                var copy = new Board(board);
                
                var outcome = copy.MakeMove(cell, move, ply);

                if (copy.IsKingInCheck(colour, colour == Colour.Black ? copy.BlackKingCell : copy.WhiteKingCell))
                {
                    _depthCounts[ply]--;
                    
                    copy.Free();
                
                    continue;
                }
                
                moved = true;

                var score = colour == Colour.Black ? copy.BlackScore : copy.WhiteScore;
                
                if (score >= _plyBestScores[ply])
                {
                    _plyBestScores[ply] = score;

                    if (depth == 1)
                    {
                        _bestPath = $"{path} {(rank, file).ToStandardNotation()}{move.ToStandardNotation()}".Trim();
                    }
                }
                
                if (perftNode == null)
                {
                    perftNode = $"{(rank, file).ToStandardNotation()}{(move >> 3, move & 7).ToStandardNotation()}";

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
                        _outcomes[ply][(int) PlyOutcome.Capture]++;
                    }

                    if (outcome == PlyOutcome.EnPassant)
                    {
                        _outcomes[ply][(int) PlyOutcome.Capture]++;

                        _outcomes[ply][(int) PlyOutcome.EnPassant]++;
                    }

                    outcome = PlyOutcome.Check;

                    if (depth == 1)
                    {
                        if (! OpponentCanMove(copy, colour.Invert()))
                        {
                            _outcomes[ply][(int) PlyOutcome.CheckMate]++;
                        }
                    }
                }

                _outcomes[ply][(int) outcome]++;

                if (outcome == PlyOutcome.EnPassant)
                {
                    _outcomes[ply][(int) PlyOutcome.Capture]++;
                }

                if (depth > 1)
                {
                    GetMoveInternal(copy, colour.Invert(), maxDepth, depth - 1, $"{path} {(rank, file).ToStandardNotation()}{move.ToStandardNotation()}", perftNode);

                    _perftCounts[perftNode]--;
                }

                if (depth == maxDepth)
                {
                    perftNode = null;
                }
                
                copy.Free();
            }
        }

        if (board.IsKingInCheck(colour, colour == Colour.Black ? board.BlackKingCell : board.WhiteKingCell) && ! moved)
        {
            if (ply == 1)
            {
                return false;
            }

            _outcomes[ply - 1][(int) PlyOutcome.CheckMate]++;
        }

        return true;
    }

    private static bool OpponentCanMove(Board board, Colour colour)
    {
        for (var cell = 0; cell < Constants.BoardCells; cell++)
        {
            var rank = cell >> 3;

            var file = cell & 7;
            
            if (board.IsEmpty(cell))
            {
                continue;
            }

            if (! board.IsColour(cell, colour))
            {
                continue;
            }

            var piece = board[rank, file];
            
            var moves = piece.GetMoves(rank, file, 1, board);

            foreach (var move in moves)
            {
                var copy = new Board(board);

                copy.MakeMove(cell, move, 1);

                if (copy.IsKingInCheck(colour, colour == Colour.Black ? copy.BlackKingCell : copy.WhiteKingCell))
                {
                    continue;
                }

                return true;
            }
        }

        return false;
    }
}