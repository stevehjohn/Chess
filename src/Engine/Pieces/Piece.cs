using Engine.Extensions;
using Engine.General;
using Engine.Infrastructure;

namespace Engine.Pieces;

public abstract class Piece
{
    public abstract Kind Kind { get; }

    public Colour Colour { get; }

    public int MoveCount { get; set; }

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

    public IEnumerable<int> GetMoves(int rank, int file, Board board)
    {
        Rank = rank;

        File = file;

        Board = board;

        return GetMoves();
    }

    protected abstract IEnumerable<int> GetMoves();

    public ushort Encode()
    {
        var code = (ushort) Kind;

        code |= (ushort) ((ushort) Colour << 3);

        code |= (ushort) (MoveCount << 5);

        return code;
    }

    public static Piece Decode(ushort code)
    {
        var kind = (Kind) (code & 0b0000_0111);

        var colour = ((code & 0b0001_1000) >> 3) switch
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
        
        piece.MoveCount = lastMovePly;

        return piece;
    }

    protected IEnumerable<int> GetDirectionalMoves(params (int RankDelta, int FileDelta)[] directions)
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
                    yield return cell;
                }

                if (Board.IsColour(cell, EnemyColour))
                {
                    yield return cell;
                    
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