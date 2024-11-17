namespace Engine.Pieces;

public class Knight : Piece
{
    private static readonly List<(int Forwards, int Right)> _moves =
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

    public Knight(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves()
    {
        foreach (var move in _moves)
        {
            if (Board.IsEmpty(Rank + move.Forwards, File + move.Right) && Board.IsColour())
            {
                yield return move;
            }
        }
    }
}