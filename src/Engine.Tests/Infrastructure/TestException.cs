namespace Engine.Tests.Infrastructure;

public class TestException : Exception
{
    public TestException(string message) : base(message)
    {
    }
}