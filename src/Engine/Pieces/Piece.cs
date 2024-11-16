using Engine.General;

namespace Engine.Pieces;

public abstract class Piece
{
    public abstract IEnumerable<int> GetMoves(int file, int rank, Board board);
}