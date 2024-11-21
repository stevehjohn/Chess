namespace Engine.Pieces;

public class Bishop : Piece
{
    public override Kind Kind => Kind.Bishop;

    public override int Value => 30;

    public Bishop(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<(int Position, bool Check)> GetMoves(int ply)
    {
        return GetDirectionalMoves((-1, -1), (-1, 1), (1, -1), (1, 1));
    }
}