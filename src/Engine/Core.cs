using Engine.General;

namespace Engine;

public class Core
{
    private Board _board;

    public void Initialise()
    {
        _board = new Board();
        
        _board.InitialisePieces();
    }
}