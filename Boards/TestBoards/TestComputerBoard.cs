namespace BattleShipConsoleGame.Boards.TestBoards
{
    internal class TestComputerBoard : ComputerBoard
    {
        public TestComputerBoard(string name = "") : base (name)
        {
            CreateEmptyBoard();
            PlaceShips(Ships.Carrier.ToString(), (int)Ships.Carrier);
            //PlaceShips(Ships.Battleship.ToString(), (int)Ships.Battleship);
            //PlaceShipsTest(Ships.Submarine1.ToString(), (int)Ships.Submarine1, 'h', new Location(1,0));
            //PlaceShips(Ships.Submarine2.ToString(), (int)Ships.Submarine2);
            //PlaceShips(Ships.Destroyer.ToString(), (int)Ships.Destroyer);
            PlaceShipsTest(Ships.Destroyer.ToString(), (int)Ships.Destroyer, 'h', new Location(7,9));
        }
        protected void PlaceShipsTest(string shipName, int shipSize, char c, Location location)
        {
            int v, h, x, y;
            bool isVerticalPlacement = true;
            if (c == 'V' || c == 'v')
                isVerticalPlacement = true;
            else if (c == 'H' || c == 'h')
                isVerticalPlacement = false;
            else
            {
                Console.WriteLine("Make sure you entered correct character. V/v or H/h");
                Environment.Exit(0);
            }
            v = location.Y;
            h = location.X;
            if (isVerticalPlacement) //if isVerticalPlacement = true it will be vertical placement
            {
                for (int i = v - 1; i < v + shipSize + 1; i++)
                    for (int j = h - 1; j < h + 2; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //controlling if it is convenient
                            {
                                Console.WriteLine("Make sure you entered valid location");
                                Environment.Exit(0);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                for (int i = 0; i < shipSize; i++)
                    GameBoard[v + i, h] = '%';
            }
            else //if isVerticalPlacement = false it will be horizontal placement
            {
                for (int i = v - 1; i < v + 2; i++)
                    for (int j = h - 1; j < h + shipSize + 1; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //controlling if it is convenient
                            {
                                Console.WriteLine("Make sure you entered valid location");
                                Environment.Exit(0);
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
        }
        public bool ShootTargetTest(char[,] targetBoard)
        {
            if (!isFinished)
            {
                Location location = GetTargetLocationTest(targetBoard);
                if (targetBoard[location.Y, location.X] == '%')
                {
                    targetBoard[location.Y, location.X] = 'X';
                    Console.WriteLine($"X = {location.X + 1}, Y = {ROWANDCOLUMN - location.Y} HIT");
                    Console.WriteLine($"PossibleShipLocations.Count = {PossibleShipLocations.Count}");
                    Console.WriteLine($"ShipHits.Count = {ShipHits.Count}");
                    Console.WriteLine($"ShipHitBorders.Count = {ShipHitBorders.Count}");
                    return true;
                }
                else if (targetBoard[location.Y, location.X] == '~')
                {
                    targetBoard[location.Y, location.X] = 'M';
                    Console.WriteLine($"X = {location.X + 1}, Y = {ROWANDCOLUMN - location.Y} MISS");
                    Console.WriteLine($"PossibleShipLocations.Count = {PossibleShipLocations.Count}");
                    Console.WriteLine($"ShipHits.Count = {ShipHits.Count}");
                    Console.WriteLine($"ShipHitBorders.Count = {ShipHitBorders.Count}");
                    return false;
                }
                else
                {
                    Console.WriteLine($"X = {location.X + 1}, Y = {ROWANDCOLUMN - location.Y} ???????");
                }
                return false;
            }
            Console.WriteLine($"Finished!");
            return false;
        }
        public Location GetTargetLocationTest(char[,] targetBoard)
        {
            int v, h, x, y;
            bool pass;
            string input;
            Location location = new Location();
        Start1:
            pass = true;
            do
            {
                Console.Write("Input location : ");
                input = Console.ReadLine();
                string[] result = input.Split(",");
                if (result.Length == 2 && int.TryParse(result[0], out x) && int.TryParse(result[1], out y))
                {
                    location = new Location(x - 1, (ROWANDCOLUMN - 1) - (y - 1));
                    pass = false;
                }
                else
                {
                    Console.WriteLine("Make sure you entered correct location.");
                }
            }
            while (pass);
            if (!HitLocations.Contains(location))
            {
                Console.WriteLine("Make sure you entered valid location");
                goto Start1;
            }
            Random random = new Random();
        Start:
            if (ShipHits.Count == 0)
            {
                PossibleShipLocations.Clear();
                if (ControlPossibleHitLocations(location, targetBoard) != -1)
                {
                    if (targetBoard[location.Y, location.X] == '%')
                    {
                        HitLocations.Remove(location);
                        ShipHits.Add(location);
                    }
                    else
                    {
                        HitLocations.Remove(location);
                    }
                }
                else
                {
                    Console.WriteLine($"X = {location.X + 1}, Y = {ROWANDCOLUMN - location.Y} REMOVED!");
                    HitLocations.Remove(location);
                    goto Start;
                }
                return location;
            }
            else if (ShipHits.Count > 0 && ShipHitBorders.Count != 2)
            {
                HitLocations.Remove(location);
                if (targetBoard[location.Y, location.X] == '%')
                {
                    ShipHits.Add(location);
                    if (ShipHits.Count > 1)
                        UpdatePossibleShipLocations(targetBoard);
                }
                else
                {
                    PossibleShipLocations.Remove(location);
                    if (ShipHits.Count > 1)
                        ShipHitBorders.Add(location);
                }
                if (ShipHitBorders.Count == 2)
                {
                    OpponentRemainShips.Remove(ShipHits.Count);
                    if (OpponentRemainShips.Count == 0)
                    {
                        isFinished = true;
                        return location;
                    }
                    RemoveImpossibleFromHitLocations();
                    ShipHits.Clear();
                    PossibleShipLocations.Clear();
                    ShipHitBorders.Clear();
                }
                return location;
            }
            return location;
        }
    }
}
