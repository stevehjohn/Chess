using Engine.Pieces;

namespace Engine.General;

public static class BoardBuilder
{
    public static Board Build()
    {
        var board = new Board();

        for (var x = 0; x < 8; x++)
        {
            board.AddPiece(Type.Pawn, new Position(1, x), Side.Black);
            board.AddPiece(Type.Pawn, new Position(6, x), Side.White);
        }

        for (var x = 0; x < 3; x++)
        {
            var type = Type.Bishop;

            switch (x)
            {
                case 0:
                    type = Type.Rook;
                    break;
                case 1:
                    type = Type.Knight;
                    break;
                case 2:
                    type = Type.Bishop;
                    break;

            }

            board.AddPiece(type, new Position(0, x), Side.Black);
            board.AddPiece(type, new Position(0, 7 - x), Side.Black);
            board.AddPiece(type, new Position(7, x), Side.White);
            board.AddPiece(type, new Position(7, 7 - x), Side.White);
        }

        board.AddPiece(Type.Queen, new Position(0, 3), Side.Black);
        board.AddPiece(Type.King, new Position(0, 4), Side.Black);

        board.AddPiece(Type.Queen, new Position(7, 3), Side.White);
        board.AddPiece(Type.King, new Position(7, 4), Side.White);

        return board;
    }
}