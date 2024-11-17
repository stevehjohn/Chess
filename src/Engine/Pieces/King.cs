using Engine.Extensions;

namespace Engine.Pieces;

public class King : Piece
{
    private static readonly List<(int Forwards, int Right)> Moves =
    [
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1)
    ];

    public override Kind Kind => Kind.King;

    public King(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves()
    {
        foreach (var move in Moves)
        {
            var cell = (Rank + move.Forwards * Direction, File + move.Right).GetCellIndex();

            if (cell < 0)
            {
                continue;
            }

            if (Board.IsEmpty(cell))
            {
                yield return cell;
            }
        }
    }
}