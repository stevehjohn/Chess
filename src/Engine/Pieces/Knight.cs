using Engine.Extensions;

namespace Engine.Pieces;

public class Knight : Piece
{
    private static readonly List<(int Forwards, int Right)> Moves =
        [
            (2, -1),
            (2, 1),
            (-2, -1),
            (-2, 1),
            (1, -2),
            (-1, -2),
            (1, 2),
            (-1, 2)
        ];
            
    public override Kind Kind => Kind.Knight;

    public override int Value => 30;

    public Knight(Colour colour) : base(colour)
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
    }
}