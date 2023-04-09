using BattleShipConsoleGame.Boards;
using BattleShipConsoleGame.Boards.TestBoards;

namespace BattleShipConsoleGame
{
    internal class Program
    {
        const int ROWANDCOLUMN = 10;
        static string s1, s2, title = "*****AMIRAL BATTI*****\n";
        static bool isGameFinished = false;
        static Board board1, board2;
        static void Main(string[] args)
        {
            PrintMainMenu();
            char input;
            s1 = $"{board1.name}{board1.board}".PadRight(board1.maxNameSize + 3);
            s2 = $"{board2.name}{board2.board}";
            Console.WriteLine(title);
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
        static void PrintMainMenu()
        {
            string[] options = { "-> ", "Play Game", "Read Rules", "Exit" };
            int current = 1;
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                Console.WriteLine(title);
                for (int i = 1; i < options.Length; i++)
                {
                    if (i == current)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(options[0] + options[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(options[i]);
                    }
                }
            GetKey:
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.DownArrow && current < 3)
                    current++;
                else if (keyInfo.Key == ConsoleKey.UpArrow && current > 1)
                    current--;
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (current == 1)
                    {
                        if (PrintPlayGame())
                            break;
                        else
                            continue;
                    }
                    else if (current == 2)
                    {

                    }
                    else if (current == 3)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                    goto GetKey;
            }
            while (true);
        }
        static bool PrintPlayGame()
        {
            string[] options = { " vs ", "/\\", "\\/", "<  User  >", "<Computer>" };
            int left = 3, right = 4, current = 0;
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                Console.WriteLine(title);
                if (current == 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(options[1].PadLeft(options[left].Length / 2 + 1));
                Console.ResetColor();
                if (current == 1)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(options[1].PadLeft(options[left].Length / 2 + options[0].Length + options[right].Length / 2));
                Console.ResetColor();
                Console.WriteLine();
                if (current == 0)
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write(options[left]);
                Console.ResetColor();
                Console.Write(options[0]);
                if (current == 1)
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write(options[right]);
                Console.ResetColor();
                Console.WriteLine();
                if (current == 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(options[2].PadLeft(options[left].Length / 2 + 1));
                Console.ResetColor();
                if (current == 1)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(options[2].PadLeft(options[left].Length / 2 + options[0].Length + options[right].Length / 2));
                Console.ResetColor();
                Console.WriteLine();
            GetKey:
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.RightArrow)
                    current = 1;
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    current = 0;
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (current == 0)
                        left = (left == 3) ? 4 : 3;
                    else if (current == 1)
                        right = (right == 3) ? 4 : 3;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (current == 0)
                        left = (left == 3) ? 4 : 3;
                    else if (current == 1)
                        right = (right == 3) ? 4 : 3;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    return false;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    string input;
                    Console.Clear();
                    Console.WriteLine(title);
                    Console.WriteLine("(Press enter to use default name)");
                    Console.Write("First board name : ");
                    input = Console.ReadLine();
                    if (left == 3)
                    {
                        input = (input == "") ? "User1" : input;
                        board1 = new UserBoard(input);
                        Console.Clear();
                        Console.WriteLine(title);
                        Console.WriteLine("(Press enter to use default name)");
                    }
                    else if (left == 4)
                    {
                        input = (input == "") ? "Computer1" : input;
                        board1 = new ComputerBoard(input);
                    }
                    Console.Write("Second board name : ");
                    input = Console.ReadLine();
                    if (right == 3)
                    {
                        input = (input == "") ? "User2" : input;
                        board2 = new UserBoard(input);
                    }
                    else if (right == 4)
                    {
                        input = (input == "") ? "Computer2" : input;
                        board2 = new ComputerBoard(input);
                    }
                    Console.Clear();
                    return true;
                }
                else
                    goto GetKey;
            }
            while (true);
        }
    }
}