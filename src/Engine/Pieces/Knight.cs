using Engine.General;

namespace Engine.Pieces;

public class Knight : Piece
{
    public override Kind Kind => Kind.Knight;

    public Knight(Colour colour) : base(colour)
    {
    }

    public override IEnumerable<int> GetPossibleMoves(int rank, int file, Board board)
    {
        throw new NotImplementedException();
    }
}