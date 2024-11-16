using Engine.General;

namespace Engine.Pieces;

public class Pawn : Piece
{
    public override Kind Kind => Kind.Pawn;

    public Pawn(Colour colour) : base(colour)
    {
    }

    public override IEnumerable<int> GetPossibleMoves()
    {
        if (LastMovePly == 0 && IsInBounds(2, 0) && ClearRankPath(2))
        {
            yield return Board.GetCellIndex(Rank + Direction * 2, File);
        }

        if (IsInBounds(1, 0) && ClearRankPath(1))
        {
            yield return Board.GetCellIndex(Rank + Direction, File);
        }
        
        // TODO: En passant. Possibly promotion? Maybe that's handled by the core.
    }
}