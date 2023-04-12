using BattleShipConsoleGame.Boards;
using BattleShipConsoleGame.Boards.TestBoards;

namespace BattleShipConsoleGame
{
    internal class Game
    {
        const int ROWANDCOLUMN = 10;
        static int time = 100;
        static string s1, s2, title = "*****AMIRAL BATTI*****\n";
        static bool isBoard1Hidden, isBoard2Hidden;
        static Board board1, board2;
        static void Main(string[] args)
        {
        Start:
            PrintMainMenu();
            s1 = $"{board1.name}{board1.board}".PadRight(board1.maxNameSize + 3);
            s2 = $"{board2.name}{board2.board}";
            while (true)
            {
                do
                {
                    if (board1.isFinished)
                    {
                        FinishGame(board1.name);
                        goto Start;
                    }
                    Console.Clear();
                    PrintGame(board1.GameBoard, board2.GameBoard);
                    Console.WriteLine($"{board1.name}'s turn. Preparing to attack {board2.name}'s board.");
                    if (board1.GetType() == typeof(ComputerBoard))
                        PauseGame();
                }
                while (board1.ShootTarget(board2.GameBoard));
                do
                {
                    if (board2.isFinished)
                    {
                        FinishGame(board2.name);
                        goto Start;
                    }
                    Console.Clear();
                    PrintGame(board1.GameBoard, board2.GameBoard);
                    Console.WriteLine($"{board2.name}'s turn. Preparing to attack {board1.name}'s board.");
                    if (board2.GetType() == typeof(ComputerBoard))
                        PauseGame();
                }
                while (board2.ShootTarget(board1.GameBoard));
            }
        }
        static void PrintGame(char[,] firstBoard, char[,] secondBoard)
        {
            Console.WriteLine(title);
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
                    if (firstBoard[i, j] == '%' && isBoard1Hidden)
                        Console.Write($"~ ");
                    else
                        Console.Write($"{firstBoard[i, j]} ");
                }
                Console.Write("|   ");
                if (i != 0)
                    Console.Write($"|{ROWANDCOLUMN - i} |");
                else
                    Console.Write($"|{ROWANDCOLUMN - i}|");
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    if (secondBoard[i, j] == '%' && isBoard2Hidden)
                        Console.Write($"~ ");
                    else
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
                        PrintRules();
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
                Console.WriteLine(options[2].PadLeft(options[left].Length / 2 + options[0].Length + options[right].Length / 2));
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("(Press left/right and up/down arrows to change board situations.)");;
                Console.WriteLine("(Press backspace to go back to main menu.)");
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
                        isBoard1Hidden = false;
                        Console.Clear();
                        Console.WriteLine(title);
                        Console.WriteLine("(Press enter to use default name)");
                    }
                    else if (left == 4)
                    {
                        input = (input == "") ? "Computer1" : input;
                        board1 = new ComputerBoard(input);
                        isBoard1Hidden = true;
                    }
                    Console.Write("Second board name : ");
                    input = Console.ReadLine();
                    if (right == 3)
                    {
                        input = (input == "") ? "User2" : input;
                        board2 = new UserBoard(input);
                        isBoard2Hidden = false;
                    }
                    else if (right == 4)
                    {
                        input = (input == "") ? "Computer2" : input;
                        board2 = new ComputerBoard(input);
                        isBoard2Hidden = true;
                    }
                    if (isBoard1Hidden && isBoard2Hidden)
                    {
                        isBoard1Hidden = false;
                        isBoard2Hidden = false;
                    }
                    else if (!isBoard1Hidden && !isBoard2Hidden)
                    {
                        isBoard1Hidden = true;
                        isBoard2Hidden = true;
                    }
                    Console.Clear();
                    return true;
                }
                else
                    goto GetKey;
            }
            while (true);
        }
        static void PrintRules()
        {
            ConsoleKeyInfo keyInfo;
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine("The Rules: ");
            Console.WriteLine("-You have 5 ships which are 5,4,3,3,2 units.");
            Console.WriteLine("-Vertical placement is top to bottom and horizontal placement is right to left from the locations that you inputted.");
            Console.WriteLine("-The ships cannot overlap and cannot touch each other.");
            Console.WriteLine();
            Console.WriteLine("(Press backspace to go back to main menu.)");
            do
            {
                keyInfo = Console.ReadKey(true);
            }
            while (keyInfo.Key != ConsoleKey.Backspace);
        }
        static void PauseGame()
        {
            ConsoleKeyInfo keyInfo;
            Console.WriteLine("(Press enter to pause the game)\n(Press left arrow to slow down, right arrow to speed up the game.)");
            for (int i = 0; i < 15; i++)
            {
                if (Console.KeyAvailable == false)
                    Thread.Sleep(time);
                else
                {
                    keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("(Press any to continue the game.)");
                        Console.ReadKey(true);
                        break;
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        time = 100;
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        time = 10;
                    }
                    else
                        Thread.Sleep(time);
                }
            }
        }
        static void FinishGame(string name)
        {
            isBoard1Hidden = false;
            isBoard2Hidden = false;
            Console.Clear();
            PrintGame(board1.GameBoard, board2.GameBoard);
            Console.WriteLine("\n******CONGRATULATIONS*******");
            Console.WriteLine($"{name} has won the game!");
            Console.WriteLine($"\nPress any to go back to main menu.");
            Console.ReadKey(true);
        }

    }
}