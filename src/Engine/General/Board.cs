using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private readonly byte[] _squares;

    public Board()
    {
        _squares = new byte[Constants.Squares];
    }

    public Kind this[int file, int rank]
    {
        set => _squares[GetSquareIndex(file, rank)] = (byte) value;
        get => (Kind) _squares[GetSquareIndex(file, rank)];
    }

    public void Initialise()
    {
        for (var file = 0; file < 8; file++)
        {
            this[file, Rank.BlackPawns] = Kind.Pawn;
        }
    }

    private static int GetSquareIndex(int file, int rank)
    {
        return file + rank * 8;
    }
}