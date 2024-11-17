namespace Engine.Pieces;

public class Knight : Piece
{
    public override Kind Kind => Kind.Knight;

    public Knight(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves()
    {
        throw new NotImplementedException();
    }
}