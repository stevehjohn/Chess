namespace Engine.General;

public class Move
{
    public Position FromPosition { get; init; }

    public Position ToPosition { get; init; }

    public int TotalValue { get; init; }

    public Move PreviousMove { get; init; }
}