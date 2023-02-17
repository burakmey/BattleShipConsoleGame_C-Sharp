using BattleShipConsoleGame.Boards;

namespace BattleShipConsoleGame
{
    internal class Program
    {
        const int ROWANDCOLUMN = 10;
        static Board user, computer1, computer2;
        static void Main(string[] args)
        {
            computer1 = new ComputerBoard();
            computer2 = new ComputerBoard();
            //PlayGame(computer1, computer2);
            char input;
            while (true)
            {
                do
                {
                    computer2.PrintBoard();
                    input = Console.ReadKey().KeyChar;
                }
                while (computer1.ShootTarget(computer2.GameBoard));
            }
        }
        static void PrintGame(char[,] firstBoard, char[,] secondBoard)
        {
            Console.WriteLine("________Your Board_______" + "   " + "______Computer Board_____");
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
        static void PlayGame(Board firstBoard, Board secondBoard)
        {
            char input;
            while (true)
            {
                do
                {
                    PrintGame(firstBoard.GameBoard, secondBoard.GameBoard);
                    input = Console.ReadKey().KeyChar;
                }
                while (firstBoard.ShootTarget(secondBoard.GameBoard));
                do
                {
                    PrintGame(firstBoard.GameBoard, secondBoard.GameBoard);
                    input = Console.ReadKey().KeyChar;
                }
                while (secondBoard.ShootTarget(firstBoard.GameBoard));
            }
        }
    }
}