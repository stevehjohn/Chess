namespace Engine.General;

public class BoardState
{
    public int BlackKingCell { get; set; }
    
    public int WhiteKingCell { get; set; }

    public BoardState()
    {
    }

    public BoardState(BoardState boardState)
    {
        BlackKingCell = boardState.BlackKingCell;

        WhiteKingCell = boardState.WhiteKingCell;
    }
}