using Engine.Extensions;
using Engine.General;
using Engine.Infrastructure;

namespace Engine.Pieces;

public abstract class Piece
{
    public abstract Kind Kind { get; }
    
    public abstract int Value { get; }

    public Colour Colour { get; }

    public int LastMovePly { get; set; }

    protected Colour EnemyColour { get; }

    protected readonly int Direction;

    protected int Rank;

    protected int File;

    protected Board Board; 

    protected Piece(Colour colour)
    {
        Colour = colour;

        EnemyColour = colour.Invert();

        Direction = colour == Colour.Black ? 1 : -1;
    }

    public IEnumerable<(int Position, bool Check)> GetMoves(int rank, int file, int ply, Board board)
    {
        Rank = rank;

        File = file;

        Board = board;

        return GetMoves(ply);
    }

    protected abstract IEnumerable<(int Position, bool Check)> GetMoves(int ply);

    public ushort Encode()
    {
        var code = (ushort) Kind;

        code |= (ushort) ((ushort) Colour << Constants.ColourBitOffset);

        code |= (ushort) (LastMovePly << Constants.LastPlyMoveBitOffset);

        return code;
    }

    public static Piece Decode(ushort code)
    {
        var kind = (Kind) (code & 0b0000_0111);

        var colour = ((code & 0b0001_1000) >> Constants.ColourBitOffset) switch
        {
            1 => Colour.White,
            2 => Colour.Black,
            _ => throw new EngineException("Invalid piece colour.")
        };

        Piece piece = kind switch
        {
            Kind.Pawn => new Pawn(colour),
            Kind.Rook => new Rook(colour),
            Kind.Knight => new Knight(colour),
            Kind.Bishop => new Bishop(colour),
            Kind.Queen => new Queen(colour),
            Kind.King => new King(colour),
            _ => throw new EngineException("Invalid piece kind.")
        };

        var lastMovePly = code & 0b1111_1111_1110_0000;
        
        piece.LastMovePly = lastMovePly;

        return piece;
    }

    protected IEnumerable<(int Position, bool Check)> GetDirectionalMoves(params (int RankDelta, int FileDelta)[] directions)
    {
        foreach (var direction in directions)
        {
            for (var distance = 1; distance <= Constants.MaxMoveDistance; distance++)
            {
                var newRank = Rank + distance * direction.RankDelta;

                var newFile = File + distance * direction.FileDelta;

                var cell = (newRank, newFile).GetCellIndex();

                if (cell < 0)
                {
                    break;
                }

                if (Board.IsEmpty(cell))
                {
                    yield return (cell, false);
                }

                if (Board.IsColour(cell, EnemyColour))
                {
                    yield return (cell, Board.CellKind(cell) == Kind.King);
                    
                    break;
                }

                if (Board.IsColour(cell, Colour))
                {
                    break;
                }
            }
        }
    }
}