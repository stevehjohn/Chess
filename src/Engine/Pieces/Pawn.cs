namespace Engine.Pieces;

public class Pawn : Piece
{
    protected override IEnumerable<int> GetMoves()
    {
        var direction = (Board[File, Rank] & Kind.Black) > 0 ? 1 : -1;

        yield return General.Board.GetSquareIndex(File, Rank + direction);
    }
}