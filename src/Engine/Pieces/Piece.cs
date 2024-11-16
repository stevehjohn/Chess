using Engine.General;
using Engine.Infrastructure;

namespace Engine.Pieces;

public abstract class Piece
{
    public abstract Kind Kind { get; }

    public Colour Colour { get; }
    
    public int LastMovePly { get; private set; }

    protected readonly int Direction;

    protected int Rank;

    protected int File;

    private Board _board;

    protected Piece(Colour colour)
    {
        Colour = colour;

        Direction = colour == Colour.Black ? 1 : -1;
    }

    public IEnumerable<int> GetPossibleMoves(int rank, int file, Board board)
    {
        Rank = rank;

        File = file;

        _board = board;

        return GetPossibleMoves();
    }

    public abstract IEnumerable<int> GetPossibleMoves();

    public ushort Encode()
    {
        var code = (ushort) Kind;

        code |= (ushort) ((ushort) Colour << 3);

        code |= (ushort) (LastMovePly << 5);

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
            Kind.Bishop => new Pawn(colour),
            Kind.Queen => new Pawn(colour),
            Kind.Kind => new Pawn(colour),
            _ => throw new EngineException("Invalid piece kind.")
        };

        var lastMovePly = code & 0b1111_1111_1110_0000;
        
        piece.LastMovePly = lastMovePly;

        return piece;
    }

    protected bool IsInBounds(int forward, int right)
    {
        var newRank = Rank + forward * Direction;

        if (newRank is < 0 or >= Constants.Ranks)
        {
            return false;
        }

        var newFile = File + right;

        if (newFile is < 0 or >= Constants.Ranks)
        {
            return false;
        }

        return true;
    }

    protected bool ClearRankPath(int forward)
    {
        var rank = Rank;
        
        while (forward > 0)
        {
            rank += Direction;

            if (IsEmpty(rank, File))
            {
                return false;
            }

            forward--;
        }

        return true;
    }

    private bool IsEmpty(int rank, int file)
    {
        return _board[rank, file] == null;
    }
}