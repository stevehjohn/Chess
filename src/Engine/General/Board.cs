using Engine.Pieces;

namespace Engine.General;

public class Board
{
    private Kind[] _squares;

    private readonly Stack<Kind[]> _undoBuffer = [];

    public Kind this[int file, int rank]
    {
        get => _squares[GetSquareIndex(file, rank)];
        private set => _squares[GetSquareIndex(file, rank)] = value;
    }

    public void Initialise()
    {
        _squares = new Kind[Constants.Squares];

        _undoBuffer.Clear();
        
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

    public void Move(int startFile, int startRank, int endFile, int endRank)
    {
        var copy = new Kind[64];
        
        Buffer.BlockCopy(_squares, 0, copy, 0, Constants.Squares);
        
        _undoBuffer.Push(copy);

        this[endFile, endRank] = this[startFile, startRank];

        this[startFile, startRank] = 0;
    }

    public void UndoMove()
    {
        _squares = _undoBuffer.Pop();
    }

    public static int GetSquareIndex(int file, int rank)
    {
        return file + rank * 8;
    }

    public static (int File, int Rank) GetRankAndFile(int square)
    {
        return (square % 8, square / 8);
    }
}