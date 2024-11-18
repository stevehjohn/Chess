using Xunit;

namespace Engine.Tests;

public class CoreTests
{
    private readonly Core _core = new();
    
    [Theory]
    [InlineData(3)]
    public void MovesPerPly(int depth)
    {
        _core.Initialise();
        
        
    }
}