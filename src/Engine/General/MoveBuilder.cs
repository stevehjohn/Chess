using Engine.Pieces;

namespace Engine.General;

public class MoveBuilder
{
    private Colour _colour;

    private int _direction;

    public MoveBuilder(Colour colour)
    {
        _colour = colour;

        _direction = _colour == Colour.Black ? 1 : 0;
    }
}