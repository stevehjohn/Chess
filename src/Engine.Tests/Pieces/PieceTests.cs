using Engine.Pieces;
using Xunit;

namespace Engine.Tests.Pieces;

public class PieceTests
{
    [Theory]
    [InlineData(0b0000_1001, Kind.Pawn, Colour.White, typeof(Pawn))]
    [InlineData(0b0000_1010, Kind.Rook, Colour.White, typeof(Rook))]
    [InlineData(0b0000_1011, Kind.Knight, Colour.White, typeof(Knight))]
    [InlineData(0b0000_1100, Kind.Bishop, Colour.White, typeof(Bishop))]
    [InlineData(0b0001_0001, Kind.Pawn, Colour.Black, typeof(Pawn))]
    [InlineData(0b0001_0010, Kind.Rook, Colour.Black, typeof(Rook))]
    [InlineData(0b0001_0011, Kind.Knight, Colour.Black, typeof(Knight))]
    [InlineData(0b0001_0100, Kind.Bishop, Colour.Black, typeof(Bishop))]
    public void DecodeReturnsCorrectPiece(ushort code, Kind expectedKind, Colour expectedColour, Type expectedType)
    {
        var piece = Piece.Decode(code);
        
        Assert.Equal(expectedKind, piece.Kind);
        
        Assert.Equal(expectedColour, piece.Colour);
        
        Assert.Equal(expectedType, piece.GetType());
    }

    [Theory]
    [InlineData(Kind.Pawn, Colour.Black, 0b0001_0001)]
    [InlineData(Kind.Rook, Colour.Black, 0b0001_0010)]
    [InlineData(Kind.Knight, Colour.Black, 0b0001_0011)]
    [InlineData(Kind.Bishop, Colour.Black, 0b0001_0100)]
    [InlineData(Kind.Pawn, Colour.White, 0b0000_1001)]
    [InlineData(Kind.Rook, Colour.White, 0b0000_1010)]
    [InlineData(Kind.Knight, Colour.White, 0b0000_1011)]
    [InlineData(Kind.Bishop, Colour.White, 0b0000_1100)]
    public void EncodeReturnsCorrectCode(Kind kind, Colour colour, ushort expectedCode)
    {
        Piece piece = kind switch
        {
            Kind.Pawn => new Pawn(colour),
            Kind.Rook => new Rook(colour),
            Kind.Knight => new Knight(colour),
            Kind.Bishop => new Bishop(colour)
        };
        
        Assert.Equal(expectedCode, piece.Encode());
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