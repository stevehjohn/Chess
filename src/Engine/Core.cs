using Engine.Extensions;
using Engine.General;
using Engine.Pieces;

namespace Engine;

public sealed class Core : IDisposable
{
    public const string EngineName = "OcpCore";

    public const string Author = "Stevo John";
    
    private Board _board;
    
    private int _ply;

    private long[] _depthCounts;

    private int[] _plyBestScores;

    private long[][] _outcomes;
    
    private CancellationTokenSource _cancellationTokenSource;

    private CancellationToken _cancellationToken;

    private Task _getMoveTask;

    private readonly Dictionary<string, long> _perftCounts = new();

    private readonly List<string> _bestPaths = [];
    
    public int MoveCount => _ply - 1;

    public long GetDepthCount(int ply) => _depthCounts[ply];

    public long GetPlyOutcome(int ply, PlyOutcome outcome) => _outcomes[ply][(int) outcome];

    public int GetBestScore(int ply) => _plyBestScores[ply];

    public long GetBestMoveCount()
    {
        long count;
        
        lock (_bestPaths)
        {
            count = _bestPaths.Count;
        }

        return count;
    }

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

    public PlyOutcome MakeMove(string move)
    {
        return MakeMove(move[..2].CellFromStandardNotation(), move[2..].CellFromStandardNotation());
    }

    public PlyOutcome MakeMove(int position, int target)
    {
        var outcome = _board.MakeMove(position, target, _ply);
        
        Player = Player.Invert();

        _ply++;

        return outcome;
    }

    public string GetMove(int depth)
    {
        _cancellationTokenSource = null;

        _getMoveTask = null;
        
        return GetMoveInternal(depth);
    }
    
    public Task GetMove(int depth, Action<string> callback)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _cancellationToken = _cancellationTokenSource.Token;

        _getMoveTask = Task.Run(() => GetMoveInternal(depth, callback), _cancellationToken);

        return _getMoveTask;
    }

    public string Interrupt()
    {
        _cancellationTokenSource.Cancel();

        lock (_bestPaths)
        {
            if (_bestPaths.Count > 0)
            {
                var path = Random.Shared.Next(_bestPaths.Count);

                return _bestPaths[path][..4];
            }
        }

        return null;
    }

    private string GetMoveInternal(int depth, Action<string> callback = null)
    {
        _depthCounts = new long[depth + 1];
        
        _plyBestScores = new int[depth + 1];

        _outcomes = new long[depth + 1][];
        
        _perftCounts.Clear();

        lock (_bestPaths)
        {
            _bestPaths.Clear();
        }

        for (var i = 1; i <= depth; i++)
        {
            _depthCounts[i] = 0;

            _plyBestScores[i] = 0;

            var outcomes = Enum.GetValuesAsUnderlyingType<PlyOutcome>();

            _outcomes[i] = new long[outcomes.Length];
        }

        string result = null;

        if (GetMoveInternal(_board, Player, depth, depth, string.Empty))
        {
            lock (_bestPaths)
            {
                if (_bestPaths.Count > 0)
                {
                    var path = Random.Shared.Next(_bestPaths.Count);

                    result = _bestPaths[path][..4];
                }
            }
        }

        if (callback != null)
        {
            callback(result);
        }

        return result;
    }

    private bool GetMoveInternal(Board board, Colour colour, int maxDepth, int depth, string path, string perftNode = null)
    {
        if (_cancellationToken.IsCancellationRequested)
        {
            return true;
        }
        
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

                var score = colour == Colour.Black 
                    ? copy.BlackScore - copy.WhiteScore
                    : copy.WhiteScore - copy.BlackScore;
                
                if (score >= _plyBestScores[ply])
                {
                    if (depth == 1)
                    {
                        lock (_bestPaths)
                        {
                            if (score > _plyBestScores[ply])
                            {
                                _plyBestScores[ply] = score;

                                _bestPaths.Clear();
                            }

                            _bestPaths.Add($"{path} {(rank, file).ToStandardNotation()}{move.ToStandardNotation()}".Trim());
                        }
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
                    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                    switch (outcome)
                    {
                        case PlyOutcome.Capture:
                            _outcomes[ply][(int) PlyOutcome.Capture]++;
                            
                            break;
                        
                        case PlyOutcome.EnPassant:
                            _outcomes[ply][(int) PlyOutcome.Capture]++;

                            _outcomes[ply][(int) PlyOutcome.EnPassant]++;
                            
                            break;
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

    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
        
        _getMoveTask?.Dispose();
    }

    public override string ToString()
    {
        return _board.ToString();
    }
}