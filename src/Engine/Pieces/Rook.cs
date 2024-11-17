namespace Engine.Pieces;

public class Rook : Piece
{
    public override Kind Kind => Kind.Rook;

    public Rook(Colour colour) : base(colour)
    {
    }
}