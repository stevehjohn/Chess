using Engine.Extensions;
using Engine.General;
using Engine.Infrastructure;
using Engine.Pieces;

namespace Engine;

public sealed class Core : IDisposable
{
    public const string EngineName = "OcpCoreEngine";

    public const string Author = "Stevo John";
    
    private Board _board;
    
    private int _ply;

    private long[] _depthCounts;

    private int[] _plyBestScores;

    private long[][] _outcomes;
    
    private List<(PlyOutcome Outcome, string Path)>[] _bestPaths = [];

    private CancellationTokenSource _cancellationTokenSource;

    private CancellationToken _cancellationToken;

    private Task _getMoveTask;
    
    private readonly Dictionary<string, long> _perftCounts = new();

    private int _lastLegalMove;
    
    public int MoveCount => _ply - 1;

    public long GetDepthCount(int ply) => _depthCounts[ply];

    public long GetPlyOutcome(int ply, PlyOutcome outcome) => _outcomes[ply][(int) outcome];

    public int GetBestScore(int ply) => _plyBestScores[ply];

    public bool IsBusy => _cancellationTokenSource != null;

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

    public (MoveOutcome Outcome, string Move) GetMove(int depth)
    {
        _cancellationTokenSource = null;

        _getMoveTask = null;
        
        return GetMoveInternal(depth);
    }
    
    public Task GetMove(int depth, Action<string> callback, int moveTime = 0)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _cancellationToken = _cancellationTokenSource.Token;

        _getMoveTask = Task.Run(() =>
        {
            var result = GetMoveInternal(depth, callback, moveTime);

            _cancellationTokenSource = null;

            _getMoveTask = null;

            return result;
            
        }, _cancellationToken);

        return _getMoveTask;
    }
    
    public string Interrupt()
    {
        if (_cancellationTokenSource == null)
        {
            throw new EngineException("No processing to interrupt.");
        }

        _cancellationTokenSource.Cancel();

        _cancellationTokenSource.Dispose();
        
        _cancellationTokenSource = null;

        _getMoveTask = null;

        return GetBestMove().Move;
    }
    
    private (MoveOutcome Outcome, string Move) GetMoveInternal(int depth, Action<string> callback = null, int moveTime = 0)
    {
        _depthCounts = new long[depth + 1];
        
        _plyBestScores = new int[depth + 1];

        _outcomes = new long[depth + 1][];

        _bestPaths = new List<(PlyOutcome Outcome, string Path)>[depth + 1];
        
        _perftCounts.Clear();

        _lastLegalMove = -1;

        for (var i = 1; i <= depth; i++)
        {
            _depthCounts[i] = 0;

            _plyBestScores[i] = 0;

            _bestPaths[i] = [];

            var outcomes = Enum.GetValuesAsUnderlyingType<PlyOutcome>();

            _outcomes[i] = new long[outcomes.Length];
        }

        GetMoveInternal(_board, Player, depth, depth, string.Empty, DateTime.UtcNow, moveTime); 

        var move = GetBestMove();

        callback?.Invoke(move.Move);

        return move;
    }

    public long GetBestMoveCount()
    {
        var bestScore = 0;

        var bestPly = 1;
            
        for (var i = 1; i < _plyBestScores.Length; i++)
        {
            if (_plyBestScores[i] > bestScore)
            {
                bestScore = _plyBestScores[i];

                bestPly = i;
            }
        }

        return _bestPaths[bestPly].Count;
    }
    
    private (MoveOutcome Outcome, string Move) GetBestMove()
    {
        var bestScore = 0;

        var bestPly = 1;
            
        for (var i = 1; i < _plyBestScores.Length; i++)
        {
            if (_plyBestScores[i] < bestScore)
            {
                break;
            }

            bestScore = _plyBestScores[i];

            bestPly = i;
        }

        if (_bestPaths[bestPly].Count > 0)
        {
            for (var i = (int) PlyOutcome.CheckMate; i >= 0; i--)
            {
                var paths = _bestPaths[bestPly].Where(p => p.Outcome == (PlyOutcome) i).ToList();
                
                if (paths.Count > 0)
                {
                    var pathIndex = Random.Shared.Next(paths.Count);

                    var path = paths[pathIndex];

                    var step = path.Path[..4];
                    
                    var stepOutcome = (PlyOutcome) i;
                    
                    if (bestPly > 1)
                    {
                        stepOutcome = _bestPaths[1].Single(m => m.Path == step).Outcome;
                    }

                    var moveOutcome = stepOutcome == PlyOutcome.CheckMate
                        ? i % 2 == 1
                            ? MoveOutcome.EngineInCheckmate
                            : MoveOutcome.OpponentInCheckmate
                        : MoveOutcome.Move;

                    return (moveOutcome, step);
                }
            }
        }

        return (MoveOutcome.Move, _lastLegalMove.ToStandardNotation());
    }

    private void GetMoveInternal(Board board, Colour colour, int maxDepth, int depth, string path, DateTime startTime, int moveTime, string perftNode = null)
    {
        if (moveTime > 0)
        {
            if ((DateTime.UtcNow - startTime).TotalMilliseconds > moveTime)
            {
                return;
            }
        }

        if (_cancellationToken.IsCancellationRequested)
        {
            return;
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
                
                var (outcome, promoted) = copy.MakeMove(cell, move, ply);

                if (copy.IsKingInCheck(colour, colour == Colour.Black ? copy.BlackKingCell : copy.WhiteKingCell))
                {
                    _depthCounts[ply]--;
                    
                    copy.Free();
                
                    continue;
                }
                
                var newCell = move > 99 ? TranslateSpecialMoveCode(cell, move) : move;

                if (newCell> 99)
                {
                    newCell= TranslateSpecialMoveCode(cell, move);
                }

                moved = true;

                if (depth == maxDepth)
                {
                    _lastLegalMove= move;
                }
                
                if (perftNode == null)
                {
                    perftNode = $"{(rank, file).ToStandardNotation()}{(newCell>> 3, newCell& 7).ToStandardNotation()}";

                    _perftCounts.TryAdd(perftNode, 1);
                }
                else
                {
                    _perftCounts[perftNode]++;
                }

                var mateFound = false;
                
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

                    if (! OpponentCanMove(copy, colour.Invert()))
                    {
                        _outcomes[ply][(int) PlyOutcome.CheckMate]++;

                        _bestPaths[ply].Add((PlyOutcome.CheckMate, $"{path} {(rank, file).ToStandardNotation()}{move.ToStandardNotation()}".Trim()));

                        mateFound = true;
                    }
                }

                if (! mateFound)
                {
                    var score = colour == Colour.Black
                        ? copy.BlackScore - copy.WhiteScore
                        : copy.WhiteScore - copy.BlackScore;

                    if (score >= _plyBestScores[ply])
                    {
                        if (score > _plyBestScores[ply])
                        {
                            _plyBestScores[ply] = score;

                            _bestPaths[ply].Clear();
                        }

                        _bestPaths[ply].Add((outcome, $"{path} {(rank, file).ToStandardNotation()}{move.ToStandardNotation()}".Trim()));
                    }
                }

                _outcomes[ply][(int) outcome]++;

                if (outcome == PlyOutcome.EnPassant)
                {
                    _outcomes[ply][(int) PlyOutcome.Capture]++;
                }

                if (promoted)
                {
                    _outcomes[ply][(int) PlyOutcome.Promotion]++;
                }

                if (depth > 1)
                {
                    GetMoveInternal(copy, colour.Invert(), maxDepth, depth - 1, $"{path} {(rank, file).ToStandardNotation()}{move.ToStandardNotation()}", startTime, moveTime, perftNode);

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
            _outcomes[ply - 1][(int) PlyOutcome.CheckMate]++;
        }
    }

    private static int TranslateSpecialMoveCode(int cell, int moveCode)
    {
        return moveCode switch
        {
            SpecialMoveCodes.CastleKingSide => cell + 2,
            SpecialMoveCodes.CastleQueenSide => cell - 2,
            SpecialMoveCodes.EnPassantUpLeft => cell - 9,
            SpecialMoveCodes.EnPassantUpRight => cell - 7,
            SpecialMoveCodes.EnPassantDownLeft => cell + 7,
            SpecialMoveCodes.EnPassantDownRight => cell + 9,
            _ => throw new EngineException($"Unknown special move code {moveCode}.")
        };
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