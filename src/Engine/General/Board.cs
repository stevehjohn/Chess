using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using Engine.Extensions;
using Engine.Infrastructure;
using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private static readonly Direction[] Directions =
    [
        new (-1, 0, true),
        new (0, -1, true),
        new (1, 0, true),
        new (0, 1, true),
        new (-1, -1, false),
        new (1, -1, false),
        new (-1, 1, false),
        new (1, 1, false)
    ];

    private static readonly Direction[] Knights =
    [
        new (2, -1, false),
        new (2, 1, false),
        new (-2, -1, false),
        new (-2, 1, false),
        new (1, -2, false),
        new (-1, -2, false),
        new (1, 2, false),
        new (-1, 2, false)
    ];

    private ushort[] _cells = new ushort[Constants.BoardCells];

    private readonly BoardState _state;

    public int BlackKingCell => _state.BlackKingCell;

    public int WhiteKingCell => _state.WhiteKingCell;

    public int BlackScore => _state.BlackScore;

    public int WhiteScore => _state.WhiteScore;

    public Board()
    {
        _state = new BoardState();
    }

    public unsafe Board(Board board)
    {
        _cells = ArrayPool<ushort>.Shared.Rent(Constants.BoardCells);

        fixed (ushort* destination = _cells)
        {
            fixed (ushort* source = board._cells)
            {
                Buffer.MemoryCopy(source, destination, Constants.BoardCells * sizeof(ushort), Constants.BoardCells * sizeof(ushort));
            }
        }

        _state = new BoardState(board._state);
    }

    public Piece this[int rank, int file]
    {
        get
        {
            var cell = _cells.Value((rank, file).GetCellIndex());

            if (cell == 0)
            {
                return null;
            }

            return Piece.Decode(cell);
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

                var i = 0;
                
                while (file < Constants.Files)
                {
                    var character = rank[i];

                    i++;
                    
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsColour(int cell, Colour colour)
    {
        return (_cells.Value(cell) & (ushort) colour << 3) > 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty(int cell)
    {
        return _cells.Value(cell) == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Kind CellKind(int cell)
    {
        return (Kind) (_cells.Value(cell) & Constants.PieceKindMask);
    }

    public int LastMovePly(int cell)
    {
        return _cells.Value(cell) >> Constants.LastPlyMoveBitOffset;
    }

    public bool LastMoveWas2Ranks(int cell)
    {
        return (_cells.Value(cell) & Constants.PawnMoved2RanksFlag) > 0;
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

    public (PlyOutcome Outcome, bool Promoted) MakeMove(int position, int target, int ply)
    {
        switch (target)
        {
            case SpecialMoveCodes.CastleKingSide:
                MovePiece(position, position + 2, ply);
            
                MovePiece(position + 3, position + 1, ply);
            
                return (PlyOutcome.Castle, false);
            
            case SpecialMoveCodes.CastleQueenSide:
                MovePiece(position, position - 2, ply);
            
                MovePiece(position - 4, position - 1, ply);
            
                return (PlyOutcome.Castle, false);
            
            case SpecialMoveCodes.EnPassantUpLeft:
                MovePiece(position, position - 9, ply);

                _cells[position - 1] = 0;

                return (PlyOutcome.EnPassant, false);
            
            case SpecialMoveCodes.EnPassantUpRight:
                MovePiece(position, position - 7, ply);

                _cells[position + 1] = 0;

                return (PlyOutcome.EnPassant, false);
            
            case SpecialMoveCodes.EnPassantDownLeft:
                MovePiece(position, position + 7, ply);

                _cells[position - 1] = 0;

                return (PlyOutcome.EnPassant, false);
            
            case SpecialMoveCodes.EnPassantDownRight:
                MovePiece(position, position + 9, ply);

                _cells[position + 1] = 0;

                return (PlyOutcome.EnPassant, false);
            
            default:
                return MovePiece(position, target, ply);
        }
    }

    public bool IsKingInCheck(Colour colour, int kingCellIndex)
    {
        var kingRank = kingCellIndex >> 3;
        
        var kingFile = kingCellIndex & 7;

        int cellRank;

        int cellFile;

        int cell;

        foreach (var direction in Directions)
        {
            cellRank = kingRank;

            cellFile = kingFile;
            
            for (var i = 0; i < Constants.MaxMoveDistance; i++)
            {
                cellRank += direction.RankDelta;

                cellFile += direction.FileDelta;

                cell = (cellRank, cellFile).GetCellIndex();

                if (cell < 0)
                {
                    break;
                }

                if (IsColour(cell, colour))
                {
                    break;
                }

                var kind = CellKind(cell);

                var isAttacking = kind switch
                {
                    Kind.Queen => true,
                    Kind.Rook => direction.IsOrthogonal,
                    Kind.Bishop => ! direction.IsOrthogonal,
                    Kind.King => i == 0,
                    _ => false
                };

                if (isAttacking)
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
            cellRank = kingRank;

            cellFile = kingFile;

            cellRank += direction.RankDelta;

            cellFile += direction.FileDelta;

            cell = (cellRank, cellFile).GetCellIndex();
        
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

        cell = (kingRank + rankDirection, kingFile - 1).GetCellIndex();

        if (cell >= 0 && IsColour(cell, colour.Invert()) && CellKind(cell) == Kind.Pawn)
        {
            return true;
        }

        cell = (kingRank + rankDirection, kingFile + 1).GetCellIndex();

        if (cell >= 0 && IsColour(cell, colour.Invert()) && CellKind(cell) == Kind.Pawn)
        {
            return true;
        }
        
        return false;
    }

    public void Free()
    {
        ArrayPool<ushort>.Shared.Return(_cells);
    }

    private (PlyOutcome Outcome, bool Promoted) MovePiece(int position, int target, int ply)
    {
        var kind = CellKind(position);
        
        var playerIsBlack = IsColour(position, Colour.Black);
        
        if (kind == Kind.King)
        {
            if (playerIsBlack)
            {
                _state.BlackKingCell = target;
            }
            else
            {
                _state.WhiteKingCell = target;
            }
        }
        
        var outcome = _cells.Value(target) > 0 ? PlyOutcome.Capture : PlyOutcome.Move;

        if (outcome == PlyOutcome.Capture)
        {
            if (playerIsBlack)
            {
                _state.BlackScore += Piece.Decode(_cells.Value(target)).Value;
            }
            else
            {
                _state.WhiteScore += Piece.Decode(_cells.Value(target)).Value;
            }
        }

        _cells[target] = _cells.Value(position);

        _cells[position] = 0;

        _cells[target] = (ushort) ((_cells.Value(target) & Constants.PieceDescriptionMask) | (ply << Constants.LastPlyMoveBitOffset));

        var promoted = false;
        
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

            if (target >> 3 is 0 or Constants.Ranks - 1)
            {
                _cells[target] &= ushort.MaxValue ^ Constants.PieceKindMask;

                // TODO: Could promote to other piece
                _cells[target] |= (ushort) Kind.Queen;

                promoted = true;
            }
        }

        return (outcome, promoted);
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