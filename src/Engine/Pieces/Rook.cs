using Engine.General;

namespace Engine.Pieces;

public class Rook : Piece
{
    public override Kind Kind => Kind.Rook;

    public Rook(Colour colour) : base(colour)
    {
    }
    
    public override IEnumerable<int> GetPossibleMoves(int rank, int file, Board board)
    {
        return [];
    }
}