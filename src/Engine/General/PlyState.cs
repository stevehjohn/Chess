using Engine.Pieces;

namespace Engine.General;

public struct PlyState
{
    public Board Board { get; }
    
    public Colour Colour { get; }
    
    public int MaxDepth { get; }
    
    public int Depth { get; }
    
    public string PerftNode { get; }

    public PlyState(Board board, Colour colour, int maxDepth, int depth, string perftNode = null)
    {
        Board = board;
        
        Colour = colour;
        
        MaxDepth = maxDepth;
        
        Depth = depth;

        PerftNode = perftNode;
    }
}