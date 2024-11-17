using Engine.Extensions;
using Engine.General;

namespace Engine.Pieces;

public class Bishop : Piece
{
    public override Kind Kind => Kind.Bishop;

    public Bishop(Colour colour) : base(colour)
    {
    }

    protected override IEnumerable<int> GetMoves()
    {
        for (var direction = 0; direction < 4; direction++)
        {
            int rankDelta, fileDelta;

            switch (direction)
            {
                case 0:
                    rankDelta = -1;
                    fileDelta = -1;
                    break;
                
                case 1:
                    rankDelta = -1;
                    fileDelta = 1;
                    break;
                
                case 2:
                    rankDelta = 1;
                    fileDelta = -1;
                    break;
                
                default:
                    rankDelta = 1;
                    fileDelta = 1;
                    break;
                
            }
            
            for (var distance = 1; distance <= Constants.MaxMoveDistance; distance++)
            {
                var newRank = Rank + distance * rankDelta;

                var newFile = File + distance * fileDelta;

                var cell = (newRank, newFile).GetCellIndex();

                if (cell < 0)
                {
                    break;
                }

                if (Board.IsEmpty(cell) || Board.IsColour(cell, EnemyColour))
                {
                    yield return cell;
                }
            }
        }
    }
}