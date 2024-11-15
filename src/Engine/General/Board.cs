using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private readonly Kind[] _squares;

    public Board()
    {
        _squares = new Kind[Constants.Squares];
    }

    public Kind this[int file, int rank]
    {
        set => _squares[GetSquareIndex(file, rank)] = value;
        get => _squares[GetSquareIndex(file, rank)];
    }

    public void Initialise()
    {
        for (var file = 0; file < 8; file++)
        {
            this[file, 1] = Kind.Pawn | Kind.Black;
            this[file, 6] = Kind.Pawn | Kind.White;
        }

        for (var file = 0; file < 3; file++)
        {
            var kind = file switch
            {
                0 => Kind.Rook,
                1 => Kind.Knight,
                _ => Kind.Bishop
            };

            this[file, 0] = kind | Kind.Black;
            this[7 - file, 0] = kind | Kind.Black;
            
            this[file, 7] = kind | Kind.White;
            this[7 - file, 7] = kind | Kind.White;
        }

        this[3, 0] = Kind.Queen | Kind.Black;
        this[4, 0] = Kind.King | Kind.Black;

        this[3, 7] = Kind.Queen | Kind.White;
        this[4, 7] = Kind.King | Kind.White;
    }

    private static int GetSquareIndex(int file, int rank)
    {
        return file * 8;
    }
}