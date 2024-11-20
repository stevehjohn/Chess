namespace Engine.General;

public static class Constants
{
    public const int BoardCells = 64;

    public const int Ranks = 8;
    public const int Files = 8;

    public const int RightmostFile = 7;
    public const int BottomRank = 7;

    public const int BlackHomeRank = 0;
    public const int BlackPawnRank = 1;

    public const int WhitePawnRank = 6;
    public const int WhiteHomeRank = 7;

    public const int LeftRookFile = 0;
    public const int LeftKnightFile = 1;
    public const int LeftBishopFile = 2;
    public const int QueenFile = 3;
    public const int KingFile = 4;
    public const int RightBishopFile = 5;
    public const int RightKnightFile = 6;
    public const int RightRookFile = 7;

    public const int MaxMoveDistance = 7;

    public const int ColourBitOffset = 3;
    public const int LastPlyMoveBitOffset = 6;
    public const ushort PawnMoved2RanksFlag = 0b0010_0000;
    public const ushort PieceKindMask = 0b0000_0111;
    public const ushort PieceDescriptionMask = 0b0001_1111;
}