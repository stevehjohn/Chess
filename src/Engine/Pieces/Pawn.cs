using Engine.Extensions;
using Engine.General;

namespace Engine.Pieces;

public class Pawn : Piece
{
    public override Kind Kind => Kind.Pawn;

    public override int Value => 10;

    public Pawn(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves(int ply)
    {
        var cell = (Rank + Direction, File).GetCellIndex();

        if (cell >= 0 && Board.IsEmpty(cell))
        {
            yield return cell;
        }

        if (LastMovePly == 0)
        {
            cell = (Rank + Direction * 2, File).GetCellIndex();

            if (cell >= 0 && Board.IsEmptyRankPath((Rank, File).GetCellIndex(), cell))
            {
                yield return cell;
            }
        }

        cell = (Rank + Direction, File - 1).GetCellIndex();

        if (cell >= 0 && Board.IsColour(cell, EnemyColour))
        {
            yield return cell;
        }

        cell = (Rank + Direction, File + 1).GetCellIndex();

        if (cell >= 0 && Board.IsColour(cell, EnemyColour))
        {
            yield return cell;
        }

        if ((Colour == Colour.White && Rank == Constants.WhitePawnRank - 3) || (Colour == Colour.Black && Rank == Constants.BlackPawnRank + 3))
        {
            cell = (Rank + Direction, File - 1).GetCellIndex();
        
            var target = cell - Direction * 8;
                
            if (cell >= 0)
            {
                if (Board.IsEmpty(cell) && Board.IsColour(target, EnemyColour) && Board.CellKind(target) == Kind.Pawn)
                {
                    if (Board.LastMovePly(target) == ply - 1 && Board.LastMoveWas2Ranks(target))
                    {
                        yield return Direction == -1 ? SpecialMoveCodes.EnPassantUpLeft : SpecialMoveCodes.EnPassantDownLeft;
                    }
                }
            }
        
            cell = (Rank + Direction, File + 1).GetCellIndex();

            target = cell - Direction * 8;

            if (cell >= 0)
            {
                if (Board.IsEmpty(cell) && Board.IsColour(target, EnemyColour) && Board.CellKind(target) == Kind.Pawn)
                {
                    if (Board.LastMovePly(target) == ply - 1 && Board.LastMoveWas2Ranks(target))
                    {
                        yield return Direction == -1 ? SpecialMoveCodes.EnPassantUpRight : SpecialMoveCodes.EnPassantDownRight;
                    }
                }
            }
        }
    }
}