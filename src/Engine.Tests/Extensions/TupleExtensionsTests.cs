using Engine.Extensions;
using Xunit;

namespace Engine.Tests.Extensions;

public class TupleExtensionsTests
{
    [Theory]
    [InlineData(0, 0, "a8")]
    [InlineData(0, 7, "h8")]
    [InlineData(7, 0, "a1")]
    [InlineData(7, 7, "h1")]
    public void ToStandardNotationReturnsExpectedValues(int rank, int file, string expected)
    {
        Assert.Equal(expected, (rank, file).ToStandardNotation());
    }
}