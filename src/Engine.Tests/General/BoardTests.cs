using System.Text;
using Engine.General;
using Engine.Pieces;
using Xunit;
using Xunit.Abstractions;

namespace Engine.Tests.General;

public class BoardTests
{
    private readonly Board _board = new();

    private readonly ITestOutputHelper _outputHelper;
    
    public BoardTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Fact]
    public void InitialiseCreatesCorrectBoardState()
    {
        _board.Initialise();

        for (var rank = 0; rank < 8; rank++)
        {
            var builder = new StringBuilder();
            
            for (var file = 0; file < 8; file++)
            {
                var character = (_board[file, rank] & Kind.TypeMask) switch
                {
                    Kind.Pawn => 'P',
                    Kind.Rook => 'R',
                    Kind.Knight => 'N',
                    Kind.Bishop => 'B',
                    Kind.Queen => 'Q',
                    Kind.King => 'K',
                    _ => '.'
                };

                if ((_board[file, rank] & Kind.ColourMask) == Kind.Black)
                {
                    character = char.ToLower(character);
                }

                builder.Append(character);
            }

            var line = builder.ToString();
            
            _outputHelper.WriteLine(line);
            
            // ReSharper disable once Xunit.XunitTestWithConsoleOutput - output doesn't show from dotnet test otherwise
            Console.WriteLine(line);

            switch (rank)
            {
                case 0:
                    Assert.Equal("rnbqkbnr", line);
                    break;

                case 1:
                    Assert.Equal("pppppppp", line);
                    break;

                case 6:
                    Assert.Equal("PPPPPPPP", line);
                    break;

                case 7:
                    Assert.Equal("RNBQKBNR", line);
                    break;
            }
        }
    }
}