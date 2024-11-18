namespace Engine.Pieces;

public class Queen : Piece
{
    public override Kind Kind => Kind.Queen;

    public Queen(Colour colour) : base(colour)
    {
    }
    
    protected override IEnumerable<int> GetMoves(int ply)
    {
        return GetDirectionalMoves((-1, 0), (0, -1), (1, 0), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1));
    }
}