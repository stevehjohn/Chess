using Engine.General;
using System.Collections.Generic;

namespace Engine.Pieces;

public abstract class Piece
{
    public Side Side { get; set; }

    public abstract Type Type { get; }

    public abstract int Value { get; }

    public int NumberOfMoves { get; set; }

    public Position Position { get; set; }

    public abstract List<Position> PossibleMoves(Board board);

    protected Piece()
    {
        NumberOfMoves = 0;
    }
}