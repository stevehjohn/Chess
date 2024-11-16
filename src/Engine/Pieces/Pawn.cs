using Engine.General;

namespace Engine.Pieces;

public class Pawn : Piece
{
    public override Kind Kind => Kind.Pawn;

    public Pawn(Colour colour) : base(colour)
    {
    }
}