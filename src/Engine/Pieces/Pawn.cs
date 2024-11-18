using Engine.Extensions;
using Engine.General;

namespace Engine.Pieces;

public class Pawn : Piece
{
    private static readonly List<(int Forwards, int Right, bool MustContainEnemy, bool FirstMoveOnly)> Moves =
    [
        (1, -1, true, false),
        (1, 1, true, false),
        (2, 0, false, true),
        (1, 0, false, false)
    ];
    
    public override Kind Kind => Kind.Pawn;

    public Pawn(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves(int ply)
    {
        foreach (var move in Moves)
        {
            var cell = (Rank + move.Forwards * Direction, File + move.Right).GetCellIndex();

            if (cell < 0)
            {
                continue;
            }

            if (LastMovePly > 0 && move.FirstMoveOnly)
            {
                continue;
            }
            
            if (move.MustContainEnemy && Board.IsColour(cell, EnemyColour))
            {
                yield return cell;
            }

            if (! move.MustContainEnemy && Board.IsEmptyRankPath((Rank, File).GetCellIndex(), cell))
            {
                yield return cell;
            }
        }
        
        if ((Colour == Colour.White && Rank == Constants.WhitePawnRank - 3) || (Colour == Colour.Black && Rank == Constants.BlackPawnRank + 3))
        {
            var cell = (Rank + Direction, File - 1).GetCellIndex();
        
            // TODO: Check last opponent was a 2fer.
            if (cell >= 0)
            {
                if (Board.IsEmpty(cell) && Board.IsColour(cell - Direction * 8, EnemyColour) && Board.CellKind(cell - Direction * 8) == Kind.Pawn)
                {
                    yield return Direction == -1 ? SpecialMoveCodes.EnPassantUpLeft : SpecialMoveCodes.EnPassantDownLeft;
                }
            }
        
            cell = (Rank + Direction, File + 1).GetCellIndex();
        
            if (cell >= 0)
            {
                if (Board.IsEmpty(cell) && Board.IsColour(cell - Direction * 8, EnemyColour) && Board.CellKind(cell - Direction * 8) == Kind.Pawn)
                {
                    yield return Direction == -1 ? SpecialMoveCodes.EnPassantUpRight : SpecialMoveCodes.EnPassantDownRight;
                }
            }
        }
    }
}