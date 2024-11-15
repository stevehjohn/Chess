using Engine.General;
using Engine.Pieces;

namespace Engine;

public class Core
{
    private readonly Board _board;

    public Core(Board board)
    {
        _board = board;
    }

    public void GetMove(Side side, int depth = 6)
    {
    }
}