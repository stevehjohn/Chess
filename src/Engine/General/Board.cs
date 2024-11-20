using System.Text;
using Engine.Extensions;
using Engine.Infrastructure;
using Engine.Pieces;

namespace Engine.General;

public class Board
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

    private ushort[] _cells = new ushort[Constants.BoardCells];

    private readonly BoardState _state;

    public int BlackKingCell => _state.BlackKingCell;

    public int WhiteKingCell => _state.WhiteKingCell;

    public Board()
    {
        _state = new BoardState();
    }

    public Board(Board board)
    {
        _cells = new ushort[Constants.BoardCells];
        
        Buffer.BlockCopy(board._cells, 0, _cells, 0, Constants.BoardCells * sizeof(ushort));

        _state = new BoardState(board._state);
    }

    public Piece this[int rank, int file]
    {
        get
        {
            var cell = _cells[(rank, file).GetCellIndex()];

            if (cell == 0)
            {
                return null;
            }

            return Piece.Decode(_cells[(rank, file).GetCellIndex()]);
        }
        set => _cells[(rank, file).GetCellIndex()] = value.Encode();
    }

    public void InitialisePieces()
    {
        _cells = new ushort[Constants.BoardCells];

        for (var file = 0; file < Constants.Files; file++)
        {
            _cells[(Constants.BlackPawnRank, file).GetCellIndex()] = new Pawn(Colour.Black).Encode();
            _cells[(Constants.WhitePawnRank, file).GetCellIndex()] = new Pawn(Colour.White).Encode();
        }

        _cells[(Constants.BlackHomeRank, Constants.LeftRookFile).GetCellIndex()] = new Rook(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.LeftKnightFile).GetCellIndex()] = new Knight(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.LeftBishopFile).GetCellIndex()] = new Bishop(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.QueenFile).GetCellIndex()] = new Queen(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.KingFile).GetCellIndex()] = new King(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.RightBishopFile).GetCellIndex()] = new Bishop(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.RightKnightFile).GetCellIndex()] = new Knight(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.RightRookFile).GetCellIndex()] = new Rook(Colour.Black).Encode();
        
        _cells[(Constants.WhiteHomeRank, Constants.LeftRookFile).GetCellIndex()] = new Rook(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.LeftKnightFile).GetCellIndex()] = new Knight(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.LeftBishopFile).GetCellIndex()] = new Bishop(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.QueenFile).GetCellIndex()] = new Queen(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.KingFile).GetCellIndex()] = new King(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.RightBishopFile).GetCellIndex()] = new Bishop(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.RightKnightFile).GetCellIndex()] = new Knight(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.RightRookFile).GetCellIndex()] = new Rook(Colour.White).Encode();

        _state.BlackKingCell = (Constants.BlackHomeRank, Constants.KingFile).GetCellIndex();
        _state.WhiteKingCell = (Constants.WhiteHomeRank, Constants.KingFile).GetCellIndex();
    }
    
    public void InitialisePieces(string fen)
    {
        try
        {
            var parts = fen.Split(' ');

            var board = parts[0];

            var ranks = board.Split('/');

            for (var rankIndex = 0; rankIndex < Constants.Ranks; rankIndex++)
            {
                var file = 0;

                var rank = ranks[rankIndex];

                while (file < rank.Length)
                {
                    var character = rank[file];
                    
                    if (char.IsNumber(character))
                    {
                        file += character - '0';
                        
                        continue;
                    }

                    var colour = char.IsUpper(character) ? Colour.White : Colour.Black;

                    Piece piece = char.ToLower(character) switch
                    {
                        'p' => new Pawn(colour),
                        'r' => new Rook(colour),
                        'n' => new Knight(colour),
                        'b' => new Bishop(colour),
                        'q' => new Queen(colour),
                        'k' => new King(colour),
                        _ => throw new EngineException("Invalid piece in FEN string.")
                    };

                    if (piece.Kind == Kind.King)
                    {
                        if (colour == Colour.Black)
                        {
                            _state.BlackKingCell = rankIndex * 8 + file;
                        }
                        else
                        {
                            _state.WhiteKingCell = rankIndex * 8 + file;
                        }
                    }

                    _cells[(rankIndex, file).GetCellIndex()] = piece.Encode();

                    file++;
                }
            }
        }
        catch (Exception exception)
        {
            throw new EngineException($"Invalid FEN string provided. {exception.Message}");
        }
    }
    
    public bool IsColour(int cell, Colour colour)
    {
        return (_cells[cell] & (ushort) colour << 3) > 0;
    }
    
    public bool IsEmpty(int cell)
    {
        return _cells[cell] == 0;
    }

    public Kind CellKind(int cell)
    {
        return (Kind) (_cells[cell] & Constants.PieceKindMask);
    }

    public int LastMovePly(int cell)
    {
        return _cells[cell] >> Constants.LastPlyMoveBitOffset;
    }

    public bool LastMoveWas2Ranks(int cell)
    {
        return (_cells[cell] & Constants.PawnMoved2RanksFlag) > 0;
    }

    public bool IsEmptyRankPath(int position, int target)
    {
        var direction = target < position ? -Constants.Files : Constants.Files;

        do
        {
            position += direction;

            if (! IsEmpty(position))
            {
                return false;
            }
            
        } while (position != target);

        return true;
    }

    public PlyOutcome MakeMove(int position, int target, int ply)
    {
        switch (target)
        {
            case SpecialMoveCodes.CastleKingSide:
                MovePiece(position, position + 2, ply);
            
                MovePiece(position + 3, position + 1, ply);
            
                return PlyOutcome.Castle;
            
            case SpecialMoveCodes.CastleQueenSide:
                MovePiece(position, position - 2, ply);
            
                MovePiece(position - 4, position - 1, ply);
            
                return PlyOutcome.Castle;
            
            case SpecialMoveCodes.EnPassantUpLeft:
                MovePiece(position, position - 9, ply);

                _cells[position - 1] = 0;

                return PlyOutcome.EnPassant;
            
            case SpecialMoveCodes.EnPassantUpRight:
                MovePiece(position, position - 7, ply);

                _cells[position + 1] = 0;

                return PlyOutcome.EnPassant;
            
            case SpecialMoveCodes.EnPassantDownLeft:
                MovePiece(position, position + 7, ply);

                _cells[position - 1] = 0;

                return PlyOutcome.EnPassant;
            
            case SpecialMoveCodes.EnPassantDownRight:
                MovePiece(position, position + 9, ply);

                _cells[position + 1] = 0;

                return PlyOutcome.EnPassant;
            
            default:
                return MovePiece(position, target, ply);
        }
    }

    public bool IsKingInCheck(Colour colour, int kingCellIndex)
    {
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

                if (IsColour(cell, colour))
                {
                    break;
                }

                var kind = CellKind(cell);

                if (kind is Kind.Rook or Kind.Queen)
                {
                    return true;
                }

                if (kind is Kind.King && i == 0)
                {
                    return true;
                }

                if (! IsEmpty(cell))
                {
                    break;
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
        
                if (IsColour(cell, colour))
                {
                    break;
                }
        
                var kind = CellKind(cell);
        
                if (kind is Kind.Bishop or Kind.Queen)
                {
                    return true;
                }
                
                if (kind is Kind.King && i == 0)
                {
                    return true;
                }

                if (! IsEmpty(cell))
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
        
            if (IsColour(cell, colour))
            {
                continue;
            }
        
            var kind = CellKind(cell);
        
            if (kind == Kind.Knight)
            {
                return true;
            }
        }

        var rankDirection = colour == Colour.Black ? 1 : -1;

        cell = (kingCell.Rank + rankDirection, kingCell.File - 1).GetCellIndex();

        if (cell >= 0 && IsColour(cell, colour.Invert()) && CellKind(cell) == Kind.Pawn)
        {
            return true;
        }

        cell = (kingCell.Rank + rankDirection, kingCell.File + 1).GetCellIndex();

        if (cell >= 0 && IsColour(cell, colour.Invert()) && CellKind(cell) == Kind.Pawn)
        {
            return true;
        }
        
        return false;
    }
    
    private PlyOutcome MovePiece(int position, int target, int ply)
    {
        if (CellKind(position) == Kind.King)
        {
            if (IsColour(position, Colour.Black))
            {
                _state.BlackKingCell = target;
            }
            else
            {
                _state.WhiteKingCell = target;
            }
        }

        var outcome = _cells[target] > 0 ? PlyOutcome.Capture : PlyOutcome.Move;

        _cells[target] = _cells[position];

        _cells[position] = 0;

        _cells[target] = (ushort) ((_cells[target] & Constants.PieceDescriptionMask) | (ply << Constants.LastPlyMoveBitOffset));

        if (CellKind(target) == Kind.Pawn)
        {
            if (Math.Abs(position - target) > 9)
            {
                _cells[target] |= Constants.PawnMoved2RanksFlag;
            }
            else
            {
                _cells[target] &= ushort.MaxValue ^ Constants.PawnMoved2RanksFlag;
            }
        }

        return outcome;
    }
    
    public override string ToString()
    {
        var builder = new StringBuilder();
            
        for (var rank = 0; rank < Constants.Ranks; rank++)
        {
            for (var file = 0; file < Constants.Files; file++)
            {
                var piece = this[rank, file];

                if (piece == null)
                {
                    builder.Append(' ');
                    
                    continue;
                }

                var character = piece.Kind switch
                {
                    Kind.Rook => 'R',
                    Kind.Knight => 'N',
                    Kind.Bishop => 'B',
                    Kind.Queen => 'Q',
                    Kind.King => 'K',
                    _ => 'P'
                };
                
                if (piece.Colour == Colour.Black)
                {
                    character = char.ToLowerInvariant(character);
                }

                builder.Append(character);
            }

            if (rank < Constants.Ranks - 1)
            {
                builder.Append('|');
            }
        }

        return builder.ToString();
    }
}