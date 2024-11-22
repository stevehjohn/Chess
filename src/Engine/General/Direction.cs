namespace Engine.General;

public struct Direction
{
    public int RankDelta { get; }
    
    public int FileDelta { get; }
    
    public bool IsOrthagonal { get; }

    public Direction(int rankDelta, int fileDelta, bool isOrthagonal)
    {
        RankDelta = rankDelta;
        
        FileDelta = fileDelta;
        
        IsOrthagonal = isOrthagonal;
    }
}