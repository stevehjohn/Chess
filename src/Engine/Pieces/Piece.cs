using Engine.General;

namespace Engine.Pieces;

public abstract class Piece
{
    protected Colour Colour;

    protected int File;

    protected int Rank;

    protected Board Board;
    
    public IEnumerable<int> GetMoves(int file, int rank, Board board)
    {
        Colour = (board[file, rank] & Kind.White) > 0 ? Colour.White : Colour.Black;

        Board = board;
        
        File = file;

        Rank = rank;
        
        return GetMoves();
    }

    protected abstract IEnumerable<int> GetMoves();
}