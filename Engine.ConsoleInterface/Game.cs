using System;
using Engine.General;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.Console;

namespace Engine.ConsoleInterface
{
    [ExcludeFromCodeCoverage]
    public class Game
    {
        private Board _board;

        public void Play()
        {
            var _board = BoardBuilder.Build();

            Title = "Chess";
            OutputEncoding = Encoding.Unicode;
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;

            Clear();

            while (true)
            {
                DisplayBoard();

                Write("\nSelect piece: ");

                ReadLine();

                Clear();
            }
        }

        private void DisplayBoard()
        {
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 8; column++)
                {

                }
            }
        }
    }
}