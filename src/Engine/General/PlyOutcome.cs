namespace Engine.General;

public enum PlyOutcome
{
    Move = 0,
    Capture = 1,
    EnPassant = 2,
    Castle = 3,
    Promotion = 4,
    Check = 5,
    CheckMate = 6
}