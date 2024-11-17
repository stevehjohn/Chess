using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private ushort[] _cells;

    private readonly Stack<ushort[]> _undoBuffer = [];

    public Piece this[int rank, int file]
    {
        get
        {
            var cell = _cells[GetCellIndex(rank, file)];

            if (cell == 0)
            {
                return null;
            }

            return Piece.Decode(_cells[GetCellIndex(rank, file)]);
        }
    }

    public void Initialise()
    {
        _cells = new ushort[Constants.BoardCells];
        
        _undoBuffer.Clear();
    }

    public void PlacePieces()
    {
        for (var file = 0; file < Constants.Files; file++)
        {
            _cells[GetCellIndex(Constants.BlackPawnRank, file)] = new Pawn(Colour.Black).Encode();
            _cells[GetCellIndex(Constants.WhitePawnRank, file)] = new Pawn(Colour.White).Encode();
        }

        _cells[GetCellIndex(Constants.BlackHomeRank, Constants.LeftRookFile)] = new Rook(Colour.Black).Encode();
        _cells[GetCellIndex(Constants.BlackHomeRank, Constants.LeftKnightFile)] = new Knight(Colour.Black).Encode();

        _cells[GetCellIndex(Constants.BlackHomeRank, Constants.RightKnightFile)] = new Knight(Colour.Black).Encode();
        _cells[GetCellIndex(Constants.BlackHomeRank, Constants.RightRookFile)] = new Rook(Colour.Black).Encode();
        
        _cells[GetCellIndex(Constants.WhiteHomeRank, Constants.LeftRookFile)] = new Rook(Colour.White).Encode();
        _cells[GetCellIndex(Constants.WhiteHomeRank, Constants.LeftKnightFile)] = new Knight(Colour.White).Encode();

        _cells[GetCellIndex(Constants.WhiteHomeRank, Constants.RightKnightFile)] = new Knight(Colour.White).Encode();
        _cells[GetCellIndex(Constants.WhiteHomeRank, Constants.RightRookFile)] = new Rook(Colour.White).Encode();
    }

    public bool IsColour(int cell, Colour colour)
    {
        return (_cells[cell] & (ushort) colour << 3) > 0;
    }
    
    public bool IsEmpty(int cell)
    {
        return _cells[cell] == 0;
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

    public static int GetCellIndex(int rank, int file)
    {
        return rank * 8 + file;
    }
}