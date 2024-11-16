using Engine.General;

namespace Engine.Pieces;

public class Rook : Piece
{
    public override IEnumerable<int> GetMoves(int file, int rank, Board board)
    {
        return [];
    }
}