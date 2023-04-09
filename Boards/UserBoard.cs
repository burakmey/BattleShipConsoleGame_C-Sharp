namespace BattleShipConsoleGame.Boards
{
    internal class UserBoard : Board
    {
        int totalShipSize = 0;
        public UserBoard(string name)
        {
            if (name.Length > maxNameSize)
                base.name = name.Substring(0, maxNameSize);
            else
                base.name = name;
            CreateEmptyBoard();
            PrintBoard();
            PlaceShips(Ships.Carrier.ToString(), (int)Ships.Carrier);
            PrintBoard();
            PlaceShips(Ships.Battleship.ToString(), (int)Ships.Battleship);
            PrintBoard();
            PlaceShips(Ships.Submarine1.ToString(), (int)Ships.Submarine1);
            PrintBoard();
            PlaceShips(Ships.Submarine2.ToString(), (int)Ships.Submarine2);
            PrintBoard();
            PlaceShips(Ships.Destroyer.ToString(), (int)Ships.Destroyer);
            PrintBoard();
            Thread.Sleep(5000);
        }
        protected override void PlaceShips(string shipName, int shipSize)
        {
            int v, h, x, y;
            bool isVerticalPlacement, pass = true;
            char c;
            string input;
            Location location = new Location();
        Start:
            pass = true;
            Console.Write("Determine the placement, vertical(V/v) or horizontal(H/h) : ");
            c = Console.ReadLine()[0];
            if (c == 'V' || c == 'v')
                isVerticalPlacement = true;
            else if (c == 'H' || c == 'h')
                isVerticalPlacement = false;
            else
            {
                Console.WriteLine("Make sure you entered correct character. V/v or H/h");
                goto Start;
            }
            do
            {
                Console.Write("Input location : ");
                input = Console.ReadLine();
                string[] result = input.Split(",");
                if (result.Length == 2 && int.TryParse(result[0], out x) && int.TryParse(result[1], out y))
                {
                    location = new Location(x, y);
                    pass = false;
                }
                else
                {
                    Console.WriteLine("Make sure you entered correct location!");
                }
            }
            while (pass);
            h = location.X - 1;
            v = (ROWANDCOLUMN - 1) - (location.Y - 1);
            if (!((isVerticalPlacement && v >= 0 && v <= ROWANDCOLUMN - shipSize && h >= 0 && h <= 9) || (!isVerticalPlacement && h >= 0 && h <= ROWANDCOLUMN - shipSize && v >= 0 && v <= 9)))
            {
                Console.WriteLine("Make sure you entered valid location!");
                goto Start;
            }
            if (isVerticalPlacement) //It will be vertical placement.
            {
                for (int i = v - 1; i < v + shipSize + 1; i++)
                    for (int j = h - 1; j < h + 2; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //Controlling if it is convenient.
                            {
                                Console.WriteLine("Make sure you entered valid location!");
                                goto Start;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                for (int i = 0; i < shipSize; i++)
                    GameBoard[v + i, h] = '%';
            }
            else //It will be horizontal placement.
            {
                for (int i = v - 1; i < v + 2; i++)
                    for (int j = h - 1; j < h + shipSize + 1; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //Controlling if it is convenient.
                            {
                                Console.WriteLine("Make sure you entered valid location!");
                                goto Start;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                for (int i = 0; i < shipSize; i++)
                    GameBoard[v, h + i] = '%';
            }
            OpponentRemainShips.Add(shipSize);
            totalShipSize += shipSize;
        }
        public override bool ShootTarget(char[,] targetBoard)
        {
            if (!isFinished)
            {
                Location location = GetTargetLocation(targetBoard);
                if (targetBoard[location.Y, location.X] == '%')
                {
                    targetBoard[location.Y, location.X] = 'X';
                    totalShipSize--;
                    if (totalShipSize == 0)
                    {
                        isFinished = true;
                    }
                    return true;
                }
                else if (targetBoard[location.Y, location.X] == '~')
                {
                    targetBoard[location.Y, location.X] = 'M';
                    return false;
                }
            }
            Console.WriteLine($"Finished!");
            return false;
        }
        public override Location GetTargetLocation(char[,] targetBoard)
        {
            int v, h, x, y;
            string input;
            Location location;
            do
            {
                Console.Write("Input location : ");
                input = Console.ReadLine();
                string[] result = input.Split(",");
                if (result.Length == 2 && int.TryParse(result[0], out x) && int.TryParse(result[1], out y))
                {
                    h = x - 1;
                    v = (ROWANDCOLUMN - 1) - (y - 1);
                    location = new Location(h, v);
                    try
                    {
                        if (targetBoard[location.Y, location.X] == '~' || targetBoard[location.Y, location.X] == '%')
                        {
                            return location;
                        }
                        else
                        {
                            Console.WriteLine("Already hit location!");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid location!");
                    }
                }
                else
                {
                    Console.WriteLine("Make sure you entered correct location.");
                }
            }
            while (true);
        }
    }
}
