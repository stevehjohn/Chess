using System;
using Engine.General;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Engine.Pieces;
using static System.Console;
using Type = Engine.Pieces.Type;

namespace Engine.ConsoleInterface
{
    [ExcludeFromCodeCoverage]
    public class Game
    {
        private Board _board;
        private ChessEngine _engine;

        public void Play()
        {
            _board = BoardBuilder.Build();
            _engine = new ChessEngine(_board);

            Title = "Chess";
            OutputEncoding = Encoding.Unicode;
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;

            Clear();

            var side = Side.White;

            while (true)
            {
                DisplayBoard();

                ForegroundColor = ConsoleColor.White;

                Write("\nPress ENTER.");

                ReadLine();

                var move = _engine.GetMove(side);

                var piece = _board.Squares[move.FromPosition.Row, move.FromPosition.Column];
                _board.Squares[move.ToPosition.Row, move.ToPosition.Column] = piece;
                _board.Squares[move.FromPosition.Row, move.FromPosition.Column] = null;

                Clear();

                side = (Side) (-(int) side);
            }
        }

        private void DisplayBoard()
        {
            Clear();
            ForegroundColor = ConsoleColor.White;
            WriteLine("\n  A B C D E F G H\n");

            for (var row = 0; row < 8; row++)
            {
                ForegroundColor = ConsoleColor.White;

                Write(row + 1);

                for (var column = 0; column < 8; column++)
                {
                    Write(' ');

                    var piece = _board.Squares[row, column];

                    if (piece == null)
                    {
                        Write(' ');
                        continue;
                    }

                    ForegroundColor = piece.Side == Side.Black
                        ? ConsoleColor.Magenta
                        : ConsoleColor.Cyan;

                    switch (piece.Type)
                    {
                        case Type.Rook:
                            Write('R');
                            break;
                        case Type.Knight:
                            Write('N');
                            break;
                        case Type.Bishop:
                            Write('B');
                            break;
                        case Type.Queen:
                            Write('Q');
                            break;
                        case Type.King:
                            Write('K');
                            break;
                        default:
                            Write('P');
                            break;
                    }
                }

                WriteLine();
            }
        }
    }
}