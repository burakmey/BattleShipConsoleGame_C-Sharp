using BattleShipConsoleGame.Boards;
using BattleShipConsoleGame.Boards.TestBoards;

namespace BattleShipConsoleGame
{
    internal class Program
    {
        const int ROWANDCOLUMN = 10;
        static string s1, s2;
        static bool isGameFinished = false;
        static Board board1, board2;
        static void Main(string[] args)
        {
            board1 = new ComputerBoard();
            board2 = new ComputerBoard();
            char input;
            s1 = $"{board1.name}{board1.board}".PadRight(board1.maxNameSize + 3);
            s2 = $"{board2.name}{board2.board}";

            while (!isGameFinished)
            {
                do
                {
                    PrintGame(board1.GameBoard, board2.GameBoard);
                    input = Console.ReadKey().KeyChar;
                }
                while (board1.ShootTarget(board2.GameBoard));
                do
                {
                    PrintGame(board1.GameBoard, board2.GameBoard);
                    input = Console.ReadKey().KeyChar;
                }
                while (board2.ShootTarget(board1.GameBoard));
            }
        }
        static void PrintGame(char[,] firstBoard, char[,] secondBoard)
        {
            Console.WriteLine(s1 + s2);
            Console.WriteLine("_________________________" + "   " + "_________________________");
            for (int i = 0; i < ROWANDCOLUMN; i++)
            {
                if (i != 0)
                    Console.Write($"|{ROWANDCOLUMN - i} |");
                else
                    Console.Write($"|{ROWANDCOLUMN - i}|");
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Console.Write($"{firstBoard[i, j]} ");
                }
                Console.Write("|   ");
                if (i != 0)
                    Console.Write($"|{ROWANDCOLUMN - i} |");
                else
                    Console.Write($"|{ROWANDCOLUMN - i}|");
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Console.Write($"{secondBoard[i, j]} ");
                }
                Console.Write("|\n");
            }
            Console.WriteLine("|yx|1|2|3|4|5|6|7|8|9|10|" + "   " + "|yx|1|2|3|4|5|6|7|8|9|10|" + "\n");
        }
    }
}