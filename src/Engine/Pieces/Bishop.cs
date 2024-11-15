namespace Engine.Pieces;

public class Bishop : Piece
{
    public override Kind Kind => Kind.Bishop;

    public Bishop(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves(int ply)
    {
        return GetDirectionalMoves((-1, -1), (-1, 1), (1, -1), (1, 1));
    }
}