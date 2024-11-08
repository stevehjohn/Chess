using Engine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Type = Engine.Pieces.Type;
// ReSharper disable StringLiteralTypo

namespace Engine.General;

public class Board
{
    public Piece[,] Squares { get; }

    public Board()
    {
        Squares = new Piece[8,8];
    }

    public void AddPiece(Type type, Position position, Side side)
    {
        // ReSharper disable once PossibleNullReferenceException
        var pieces = Assembly
            .GetAssembly(typeof(Piece))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Piece)));

        foreach (var piece in pieces)
        {
            var instance = (Piece) Activator.CreateInstance(piece);

            // ReSharper disable once PossibleNullReferenceException
            if (instance.Type == type)
            {
                instance.Position = position;
                instance.Side = side;

                Squares[position.Row, position.Column] = instance;
                return;
            }
        }
    }

    public string[] Dump()
    {
        var result = new List<string>();
 
        for (var row = 0; row < 8; row++)
        {
            var sb = new StringBuilder();

            for (var column = 0; column < 8; column++)
            {
                if (Squares[row, column] == null)
                {
                    sb.Append(" ");
                    continue;
                }

                var piece = ' ';

                switch (Squares[row, column].Type)
                {
                    case Type.Bishop:
                        piece = '♗';
                        break;
                    case Type.King:
                        piece = '♔';
                        break;
                    case Type.Knight:
                        piece = '♘';
                        break;
                    case Type.Pawn:
                        piece = '♙';
                        break;
                    case Type.Queen:
                        piece = '♕';
                        break;
                    case Type.Rook:
                        piece ='♖';
                        break;
                }

                if (Squares[row, column].Side == Side.Black)
                {
                    piece += '\u0006';
                }

                sb.Append(piece);
            }

            result.Add(sb.ToString());
        }

        return result.ToArray();
    }
}