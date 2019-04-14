using System.Diagnostics.CodeAnalysis;

namespace Engine.ConsoleInterface
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game();

            game.Play();
        }
    }
}
