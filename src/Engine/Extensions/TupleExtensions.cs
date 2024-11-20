using Engine.General;

namespace Engine.Extensions;

public static class TupleExtensions
{
    public static int GetCellIndex(this (int Rank, int File) position)
    {
        if (position.Rank is < 0 or >= Constants.Ranks || position.File is < 0 or >= Constants.Files)
        {
            return int.MinValue;
        }

        var cell = position.Rank * 8 + position.File;

        return cell;
    }

    public static string ToStandardNotation(this (int Rank, int File) position)
    {
        return $"{(char) ('a' + position.File)}{8 - position.Rank}";
    }
}