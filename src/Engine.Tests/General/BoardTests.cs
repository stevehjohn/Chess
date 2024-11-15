using Engine.General;
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
    }
}