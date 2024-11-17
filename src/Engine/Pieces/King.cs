namespace Engine.Pieces;

public class King : Piece
{
    public override Kind Kind => Kind.King;

    public King(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves()
    {
        return [];
    }
}