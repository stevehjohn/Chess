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
    [InlineData(0b0000_1101, Kind.Queen, Colour.White, typeof(Queen))]
    [InlineData(0b0000_1110, Kind.King, Colour.White, typeof(King))]
    [InlineData(0b0001_0001, Kind.Pawn, Colour.Black, typeof(Pawn))]
    [InlineData(0b0001_0010, Kind.Rook, Colour.Black, typeof(Rook))]
    [InlineData(0b0001_0011, Kind.Knight, Colour.Black, typeof(Knight))]
    [InlineData(0b0001_0100, Kind.Bishop, Colour.Black, typeof(Bishop))]
    [InlineData(0b0001_0101, Kind.Queen, Colour.Black, typeof(Queen))]
    [InlineData(0b0001_0110, Kind.King, Colour.Black, typeof(King))]
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
    [InlineData(Kind.Queen, Colour.Black, 0b0001_0101)]
    [InlineData(Kind.King, Colour.Black, 0b0001_0110)]
    [InlineData(Kind.Pawn, Colour.White, 0b0000_1001)]
    [InlineData(Kind.Rook, Colour.White, 0b0000_1010)]
    [InlineData(Kind.Knight, Colour.White, 0b0000_1011)]
    [InlineData(Kind.Bishop, Colour.White, 0b0000_1100)]
    [InlineData(Kind.Queen, Colour.White, 0b0000_1101)]
    [InlineData(Kind.King, Colour.White, 0b0000_1110)]
    [InlineData((Kind) 7, Colour.White, 0, true)]
    public void EncodeReturnsCorrectCode(Kind kind, Colour colour, ushort expectedCode, bool expectException = false)
    {
        Piece piece = null;
        
        try
        {
            piece = kind switch
            {
                Kind.Pawn => new Pawn(colour),
                Kind.Rook => new Rook(colour),
                Kind.Knight => new Knight(colour),
                Kind.Bishop => new Bishop(colour),
                Kind.Queen => new Queen(colour),
                Kind.King => new King(colour),
                _ => throw new TestException("Invalid kind specified.")
            };
        }
        catch
        {
            if (expectException)
            {
                return;
            }
        }

        if (expectException)
        {
            Assert.Fail();
        }

        Assert.NotNull(piece);

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