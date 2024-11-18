using Engine.Pieces;

namespace Engine.Extensions;

public static class ColourExtensions
{
    public static Colour Invert(this Colour colour)
    {
        return 3 - colour;
    }
}