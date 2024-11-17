namespace Engine.Pieces;

public class Queen : Piece
{
    public override Kind Kind => Kind.Queen;

    public Queen(Colour colour) : base(colour)
    {
    }
    
    protected override IEnumerable<int> GetMoves()
    {
        return [];
    }
}