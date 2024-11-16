using Engine.Pieces;

namespace Engine.General;

public class MoveBuilder
{
    private Kind _kind;

    private Colour _colour;

    public MoveBuilder(Kind kind, Colour colour)
    {
        _kind = kind;
        
        _colour = colour;
    }
}