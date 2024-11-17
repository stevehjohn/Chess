using Engine.Extensions;
using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private ushort[] _cells = new ushort[Constants.BoardCells];

    private readonly Stack<ushort[]> _undoBuffer = [];

    public Piece this[int rank, int file]
    {
        get
        {
            var cell = _cells[(rank, file).GetCellIndex()];

            if (cell == 0)
            {
                return null;
            }

            return Piece.Decode(_cells[(rank, file).GetCellIndex()]);
        }
    }

    public void InitialisePieces()
    {
        _cells = new ushort[Constants.BoardCells];
        
        _undoBuffer.Clear();

        for (var file = 0; file < Constants.Files; file++)
        {
            _cells[(Constants.BlackPawnRank, file).GetCellIndex()] = new Pawn(Colour.Black).Encode();
            _cells[(Constants.WhitePawnRank, file).GetCellIndex()] = new Pawn(Colour.White).Encode();
        }

        _cells[(Constants.BlackHomeRank, Constants.LeftRookFile).GetCellIndex()] = new Rook(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.LeftKnightFile).GetCellIndex()] = new Knight(Colour.Black).Encode();

        _cells[(Constants.BlackHomeRank, Constants.RightKnightFile).GetCellIndex()] = new Knight(Colour.Black).Encode();
        _cells[(Constants.BlackHomeRank, Constants.RightRookFile).GetCellIndex()] = new Rook(Colour.Black).Encode();
        
        _cells[(Constants.WhiteHomeRank, Constants.LeftRookFile).GetCellIndex()] = new Rook(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.LeftKnightFile).GetCellIndex()] = new Knight(Colour.White).Encode();

        _cells[(Constants.WhiteHomeRank, Constants.RightKnightFile).GetCellIndex()] = new Knight(Colour.White).Encode();
        _cells[(Constants.WhiteHomeRank, Constants.RightRookFile).GetCellIndex()] = new Rook(Colour.White).Encode();
    }

    public bool IsColour(int cell, Colour colour)
    {
        return (_cells[cell] & (ushort) colour << 3) > 0;
    }
    
    public bool IsEmpty(int cell)
    {
        return _cells[cell] == 0;
    }

    public bool IsEmptyRankPath(int position, int target)
    {
        var direction = target < position ? -Constants.Files : Constants.Files;

        do
        {
            position += direction;

            if (! IsEmpty(position))
            {
                return false;
            }
            
        } while (position != target);

        return true;
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