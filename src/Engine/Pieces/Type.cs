namespace Engine.Pieces;

public enum Type : byte
{
    Pawn   = 0b0000_0001,
    Rook   = 0b0000_0010,
    Knight = 0b0000_0011,
    Bishop = 0b0000_0100,
    Queen  = 0b0000_0101,
    King   = 0b0000_0111
}