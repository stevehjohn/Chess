namespace Engine.Pieces;

public class Rook : Piece
{
    public override Kind Kind => Kind.Rook;

    public Rook(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves(int ply)
    {
        return GetDirectionalMoves((-1, 0), (0, -1), (1, 0), (0, 1));
    }
}