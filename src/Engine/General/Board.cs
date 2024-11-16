namespace Engine.General;

public class Board
{
    private ushort[] _cells;

    private readonly Stack<ushort[]> _undoBuffer = [];
    
    public void Initialise()
    {
        _cells = new ushort[Constants.BoardCells];
        
        _undoBuffer.Clear();
    }

    public void MakeMove()
    {
        var copy = new ushort[Constants.BoardCells];
        
        Buffer.BlockCopy(_cells, 0, copy, 0, Constants.BoardCells);
        
        _undoBuffer.Push(copy);
        
        // TODO: The move
    }

    public void UndoMove()
    {
        _cells = _undoBuffer.Pop();
    }
}