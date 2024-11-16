using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private ushort[] _cells;

    private readonly Stack<ushort[]> _undoBuffer = [];
    
    public Piece this[int rank, int file] => Piece.Decode(_cells[GetCellIndex(rank, file)]);
    
    public void Initialise()
    {
        _cells = new ushort[Constants.BoardCells];
        
        _undoBuffer.Clear();

        for (var file = 0; file < Constants.Files; file++)
        {
            _cells[GetCellIndex(Constants.BlackPawnRank, file)] = new Pawn(Colour.Black).Encode();
            _cells[GetCellIndex(Constants.WhitePawnRank, file)] = new Pawn(Colour.White).Encode();
        }

        _cells[GetCellIndex(Constants.BlackHomeRank, Constants.LeftRookFile)] = new Rook(Colour.Black).Encode();

        _cells[GetCellIndex(Constants.BlackHomeRank, Constants.RightRookFile)] = new Rook(Colour.Black).Encode();
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

    private static int GetCellIndex(int rank, int file)
    {
        return rank * 8 + file;
    }
}