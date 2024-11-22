namespace Engine.General;

public struct Direction
{
    public int RankDelta { get; }
    
    public int FileDelta { get; }
    
    public bool IsOrthogonal { get; }

    public Direction(int rankDelta, int fileDelta, bool isOrthogonal)
    {
        RankDelta = rankDelta;
        
        FileDelta = fileDelta;
        
        IsOrthogonal = isOrthogonal;
    }
}