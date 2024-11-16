namespace Engine.Pieces;

public abstract class Piece
{
    public abstract Kind Kind { get; }

    public abstract Colour Colour { get; }

    public ushort Encode()
    {
        var code = (ushort) Kind;

        code |= (ushort) ((ushort) Colour << 3);

        return code;
    }

    public Piece Decode(ushort code)
    {
        var kind = (Kind) (code & 0b0000_0111);

        var colour = (Colour) (code & 0b0001_1000);

        var piece = kind switch
        {
            Kind.Pawn => new Pawn()
        };
    }
}