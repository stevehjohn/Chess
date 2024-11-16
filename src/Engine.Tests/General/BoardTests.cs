using Engine.General;
using Engine.Pieces;
using Xunit;

namespace Engine.Tests.General;

public class BoardTests
{
    private readonly Board _board = new();
    
    [Theory]
    [InlineData(Constants.BlackPawnRank, 0, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 1, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 2, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 3, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 4, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 5, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 6, Kind.Pawn, Colour.Black)]
    [InlineData(Constants.BlackPawnRank, 7, Kind.Pawn, Colour.Black)]

    [InlineData(Constants.BlackHomeRank, Constants.LeftRookFile, Kind.Rook, Colour.Black)]
    [InlineData(Constants.BlackHomeRank, Constants.RightRookFile, Kind.Rook, Colour.Black)]

    [InlineData(Constants.WhitePawnRank, 0, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 1, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 2, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 3, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 4, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 5, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 6, Kind.Pawn, Colour.White)]
    [InlineData(Constants.WhitePawnRank, 7, Kind.Pawn, Colour.White)]
    public void InitialiseSetsUpBoardCorrectly(int rank, int file, Kind expectedKind, Colour expectedColour)
    {
        _board.Initialise();

        var piece = _board[rank, file];
        
        Assert.Equal(expectedKind, piece.Kind);
        Assert.Equal(expectedColour, piece.Colour);
    }
}