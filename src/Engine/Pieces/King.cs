using Engine.Extensions;
using Engine.General;

namespace Engine.Pieces;

public class King : Piece
{
    private static readonly List<(int Forwards, int Right)> Moves =
    [
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1),
        (-1, -1),
        (1, 1),
        (-1, 1),
        (1, -1)
    ];

    public override Kind Kind => Kind.King;

    public override int Value => int.MaxValue;

    public King(Colour colour) : base(colour)
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

            if (Board.IsEmpty(cell) || Board.IsColour(cell, EnemyColour))
            {
                yield return cell;
            }
        }

        if (LastMovePly == 0)
        {
            if (CheckCanCastleKingSide())
            {
                yield return SpecialMoveCodes.CastleKingSide;
            }

            if (CheckCanCastleQueenSide())
            {
                yield return SpecialMoveCodes.CastleQueenSide;
            }
        }
    }

    private bool CheckCanCastleKingSide()
    {
        var cell = (Rank, Constants.RightRookFile).GetCellIndex();

        if (Board.IsKingInCheck(Colour, (Rank, File).GetCellIndex()))
        {
            return false;
        }

        if (Board.IsColour(cell, Colour) && Board.CellKind(cell) == Kind.Rook && Board.LastMovePly(cell) == 0)
        {
            if (Board.IsEmpty((Rank, Constants.RightBishopFile).GetCellIndex()) 
                && Board.IsEmpty((Rank, Constants.RightKnightFile).GetCellIndex()))
            {
                if (! Board.IsKingInCheck(Colour, (Rank, Constants.RightBishopFile).GetCellIndex()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckCanCastleQueenSide()
    {
        var cell = (Rank, Constants.LeftRookFile).GetCellIndex();
        
        if (Board.IsKingInCheck(Colour, (Rank, File).GetCellIndex()))
        {
            return false;
        }

        if (Board.IsColour(cell, Colour) && Board.CellKind(cell) == Kind.Rook && Board.LastMovePly(cell) == 0)
        {
            if (Board.IsEmpty((Rank, Constants.QueenFile).GetCellIndex()) 
                && Board.IsEmpty((Rank, Constants.LeftBishopFile).GetCellIndex())
                && Board.IsEmpty((Rank, Constants.LeftKnightFile).GetCellIndex()))
            {
                if (! Board.IsKingInCheck(Colour, (Rank, Constants.QueenFile).GetCellIndex()))
                {
                    return true;
                }
            }
        }

        return false;
    }
}