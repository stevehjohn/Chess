using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class PieceTests
{
    [Theory]
    [InlineData(0b0000_1001, Kind.Pawn, Colour.White, typeof(Pawn))]
    public void DecodeReturnsCorrectPiece(ushort code, Kind expectedKind, Colour expectedColour, Type expectedType)
    {
        var piece = Piece.Decode(code);
        
        Assert.Equal(expectedKind, piece.Kind);
        
        Assert.Equal(expectedColour, piece.Colour);
        
        Assert.Equal(expectedType, piece.GetType());
    }
    
    [Theory]
    [InlineData(0b0000_0000, "Invalid piece colour.")]
    [InlineData(0b0001_1001, "Invalid piece colour.")]
    [InlineData(0b0000_1111, "Invalid piece kind.")]
    public void DecodeReturnsThrowsExceptionForInvalidCodes(ushort code, string expectedMessage)
    {
        try
        {
            Piece.Decode(code);
        }
        catch (Exception exception)
        {
            Assert.Equal(expectedMessage, exception.Message);
            
            return;
        }
        
        Assert.Fail("Exception not thrown parsing code.");
    }
}