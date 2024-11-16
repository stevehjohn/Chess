using Engine.General;

namespace Engine.Pieces;

public interface IPiece
{
    IEnumerable<int> GetMoves(int file, int rank, Board board);
}