using Engine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Type = Engine.Pieces.Type;

namespace Engine.General
{
    public class Board
    {
        public Piece[,] Squares { get; set; }

        public Board()
        {
            Squares = new Piece[8,8];
        }

        public void AddPiece(Type type, Position position, Side side)
        {
            var pieces = Assembly
                         .GetAssembly(typeof(Piece))
                         .GetTypes()
                         .Where(t => t.IsSubclassOf(typeof(Piece)));

            foreach (var piece in pieces)
            {
                var instance = (Piece) Activator.CreateInstance(piece);

                if (instance.Type == type)
                {
                    instance.Position = position;
                    instance.Side = side;

                    Squares[position.Column, position.Row] = instance;
                    return;
                }
            }
        }

        public string[] Dump()
        {
            var result = new List<string>();

            for (var y = 0; y < 8; y++)
            {
                var sb = new StringBuilder();

                for (var x = 0; x < 8; x++)
                {
                    if (Squares[x, y] == null)
                    {
                        sb.Append("-");
                        continue;
                    }

                    switch (Squares[x, y].Type)
                    {
                        case Type.Bishop:
                            sb.Append("B");
                            break;
                        case Type.King:
                            sb.Append("K");
                            break;
                        case Type.Knight:
                            sb.Append("N");
                            break;
                        case Type.Pawn:
                            sb.Append("P");
                            break;
                        case Type.Queen:
                            sb.Append("Q");
                            break;
                        case Type.Rook:
                            sb.Append("R");
                            break;
                    }
                }

                result.Add(sb.ToString());
            }

            return result.ToArray();
        }
    }
}