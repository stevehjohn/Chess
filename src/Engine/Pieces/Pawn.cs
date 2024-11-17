using Engine.Extensions;

namespace Engine.Pieces;

public class Pawn : Piece
{
    public override Kind Kind => Kind.Pawn;

    public Pawn(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves()
    {
        int cell;
        
        if (LastMovePly == 0)
        {
            cell = (Rank + Direction * 2, File).GetCellIndex();

            if (cell < 0 && Board.IsEmptyRankPath((Rank + Direction * 2, File).GetCellIndex(), cell))
            {
                yield return cell;
            }
        }

        cell = (Rank + Direction * 2, File).GetCellIndex();

        if (Board.IsEmpty(cell))
        {
            yield return cell;
        }
    }
}