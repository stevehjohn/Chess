using Engine.Infrastructure;

namespace Engine.Pieces;

public abstract class Piece
{
    public abstract Kind Kind { get; }

    public Colour Colour { get; }

    protected Piece(Colour colour)
    {
        Colour = colour;
    }
    
    public ushort Encode()
    {
        var code = (ushort) Kind;

        code |= (ushort) ((ushort) Colour << 3);

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
        
        var piece = kind switch
        {
            Kind.Pawn => new Pawn(colour),
            Kind.Rook => new Pawn(colour),
            Kind.Knight => new Pawn(colour),
            Kind.Bishop => new Pawn(colour),
            Kind.Queen => new Pawn(colour),
            Kind.Kind => new Pawn(colour),
            _ => throw new EngineException("Invalid piece kind.")
        };

        return piece;
    }
}