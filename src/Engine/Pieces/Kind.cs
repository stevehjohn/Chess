namespace Engine.Pieces;

[Flags]
public enum Kind : byte
{
    Pawn   = 0b0000_0001,
    Rook   = 0b0000_0010,
    Knight = 0b0000_0011,
    Bishop = 0b0000_0100,
    Queen  = 0b0000_0101,
    King   = 0b0000_0111,
    Black  = 0b0000_1000,
    White  = 0b0001_0000
}