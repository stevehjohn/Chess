using Engine.General;

namespace Engine.Pieces;

public class Pawn : Piece
{
    public override Kind Kind => Kind.Pawn;

    public Pawn(Colour colour) : base(colour)
    {
    }

    public override IEnumerable<int> GetPossibleMoves(int rank, int file, Board board)
    {
        if (LastMovePly == 0 && IsValidMove(rank, file, 2, 0))
        {
            yield return Board.GetCellIndex(rank + Direction, file);
        }
    }
}