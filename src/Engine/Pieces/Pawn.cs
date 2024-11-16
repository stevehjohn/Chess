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
        if (LastMovePly == 0 && IsInBounds(rank, file, 2, 0) && ClearRankPath(rank, file, 2, board))
        {
            yield return Board.GetCellIndex(rank + Direction * 2, file);
        }

        if (IsInBounds(rank, file, 1, 0) && ClearRankPath(rank, file, 1, board))
        {
            yield return Board.GetCellIndex(rank + Direction, file);
        }
        
        // TODO: En passant. Possibly promotion? Maybe that's handled by the core.
    }
}