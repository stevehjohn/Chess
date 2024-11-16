using Engine.General;

namespace Engine.Pieces;

public class King : Piece
{
    public override IEnumerable<int> GetMoves(int file, int rank, Board board)
    {
        return [];
    }
}