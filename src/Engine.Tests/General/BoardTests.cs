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

        for (var file = 0; file < 8; file++)
        {
            var builder = new StringBuilder();
            
            for (var rank = 0; rank < 8; rank++)
            {
                var character = _board[file, rank] switch
                {
                    Kind.Pawn => 'P',
                    _ => 'x'
                };

                builder.Append(character);
            }

            var line = builder.ToString();
            
            _outputHelper.WriteLine(line);
            
            // ReSharper disable once Xunit.XunitTestWithConsoleOutput - output doesn't show from dotnet test otherwise
            Console.WriteLine(line);
        }
    }
}