using System.Drawing;

namespace BattleShipConsoleGame.Boards
{
    internal class ComputerBoard : Board
    {
        string name = "Computer Board";
        int maxNameSize = 23;
        bool isVerticalPlacement;
        int ship;
        List<Location> hitLocations;
        List<Location> possibleShipLocations;
        List<Location> shipHits;
        List<Location> shipHitBorders;
        public List<Location> HitLocations
        {
            get { return hitLocations; }
            set { hitLocations = value; }
        }
        public List<Location> PossibleShipLocations
        {
            get { return possibleShipLocations; }
            set { possibleShipLocations = value; }
        }
        public List<Location> ShipHits
        {
            get { return shipHits; }
            set { shipHits = value; }
        }
        public List<Location> ShipHitBorders
        {
            get { return shipHitBorders; }
            set { shipHitBorders = value; }
        }
        public ComputerBoard()
        {
            CreateEmptyBoard();
            PlaceShips();
            PlaceShips();
            PlaceShips();
            PlaceShips();
            PlaceShips();
        }
        public override void CreateEmptyBoard()
        {
            GameBoard = new char[ROWANDCOLUMN, ROWANDCOLUMN];
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    GameBoard[i, j] = '~';

            OpponentRemainShips = new List<int>();
            foreach (int value in Enum.GetValues<Ships>())
                OpponentRemainShips.Add(value);

            PlacementShips = new List<int>(OpponentRemainShips);

            PossibleShipLocations = new List<Location>();
            ShipHits = new List<Location>();
            ShipHitBorders = new List<Location>();
            HitLocations = new List<Location>();
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    HitLocations.Add(new Location(i, j));
        }
        public override void PrintBoard()
        {
            Console.WriteLine("_____Computer Board______");
            for (int i = 0; i < ROWANDCOLUMN; i++)
            {
                if (i != 0)
                    Console.Write($"|{ROWANDCOLUMN - i} |");
                else
                    Console.Write($"|{ROWANDCOLUMN - i}|");
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Console.Write($"{GameBoard[i, j]} ");
                }
                Console.Write("|\n");
            }
            Console.WriteLine("|yx|1|2|3|4|5|6|7|8|9|10|" + "\n");
        }
        protected override void PlaceShips()
        {
            int v, h;
            Random random = new Random();
            ship = PlacementShips.First();
        Start:
            isVerticalPlacement = random.Next(2) == 0 ? true : false;
            Location location = GetPlacementLocation();
            h = location.X;
            v = location.Y;
            if (isVerticalPlacement) //if isVerticalPlacement = true it will be vertical placement
            {
                for (int i = v - 1; i < v + ship + 1; i++)
                    for (int j = h - 1; j < h + 2; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //controlling if it is convenient
                                goto Start;
                        }
                        catch (Exception)
                        {
                        }
                    }
                for (int i = 0; i < ship; i++)
                    GameBoard[v + i, h] = '%';
            }
            else //if isVerticalPlacement = false it will be horizontal placement
            {
                for (int i = v - 1; i < v + 2; i++)
                    for (int j = h - 1; j < h + ship + 1; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //controlling if it is convenient
                                goto Start;
                        }
                        catch (Exception)
                        {
                        }
                    }
                for (int i = 0; i < ship; i++)
                    GameBoard[v, h + i] = '%';
            }
            PlacementShips.Remove(ship);
        }
        protected override Location GetPlacementLocation()
        {
            Location location = new Location();
            Random random = new Random();
            isVerticalPlacement = random.Next(2) == 0 ? true : false;
            if (isVerticalPlacement)
            {
                location = new Location(random.Next(ROWANDCOLUMN), random.Next(ROWANDCOLUMN - ship + 1));
            }
            else
            {
                location = new Location(random.Next(ROWANDCOLUMN - ship + 1), random.Next(ROWANDCOLUMN));
            }
            return location;
        }
        public override bool ShootTarget(char[,] targetBoard)
        {
            Location location = GetTargetLocation(targetBoard);
            if (!isFinished)
            {
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
        public override Location GetTargetLocation(char[,] targetBoard)
        {
            Random random = new Random();
            int index;
            Location location;
        Start:
            if (ShipHits.Count == 0)
            {
                index = random.Next(HitLocations.Count);
                location = HitLocations[index];
                HitLocations.Remove(location);
                if (targetBoard[location.Y, location.X] == '%')
                {
                    ShipHits.Add(location);
                    AddToPossibleShipLocations(location, targetBoard);    
                }
                return location;
            }
            else if (ShipHits.Count > 0 && ShipHitBorders.Count != 2 && OpponentRemainShips.Max() > ShipHits.Count)
            {
                index = random.Next(PossibleShipLocations.Count);
                location = PossibleShipLocations[index];
                HitLocations.Remove(location);
                if (targetBoard[location.Y, location.X] == '%')
                {
                    ShipHits.Add(location);
                    PossibleShipLocations.Clear();
                    UpdatePossibleShipLocations();
                }
                else
                {
                    PossibleShipLocations.Remove(location);
                    if (ShipHits.Count > 1)
                    {
                        //ShipHitBorders.Add(location);
                        PossibleShipLocations.Clear();
                        UpdatePossibleShipLocations();
                    }
                }
                return location;
            }
            else
            {
                OpponentRemainShips.Remove(ShipHits.Count);
                if (OpponentRemainShips.Count == 0)
                {
                    isFinished = true;
                    return new Location();
                }
                RemoveImpossibleFromHitLocations();
                ShipHits.Clear();
                PossibleShipLocations.Clear();
                ShipHitBorders.Clear();
                goto Start;
            }
        }
        void AddToPossibleShipLocations(Location location, char[,] targetBoard)
        {
            List<int> sizes = new List<int> { 1, 1, 1, 1 };
            for (int i = location.Y - 1; ; i--)
            {
                try
                {
                    if (targetBoard[i, location.X] != '~' && !HitLocations.Contains(new Location(location.X, i)))
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
                sizes[0]++;
                if (sizes[0] == OpponentRemainShips.Max())
                    break;
            }
            if (sizes[0] >= OpponentRemainShips.Min() && HitLocations.Contains(new Location(location.X, location.Y - 1)))
                PossibleShipLocations.Add(new Location(location.X, location.Y - 1));
            for (int j = location.X + 1; ; j++)
            {
                try
                {
                    if (targetBoard[location.Y, j] != '~' && !HitLocations.Contains(new Location(j, location.Y)))
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
                sizes[1]++;
                if (sizes[1] == OpponentRemainShips.Max())
                    break;
            }
            if (sizes[1] >= OpponentRemainShips.Min() && HitLocations.Contains(new Location(location.X + 1, location.Y)))
                PossibleShipLocations.Add(new Location(location.X + 1, location.Y));
            for (int i = location.Y + 1; ; i++)
            {
                try
                {
                    if (targetBoard[i, location.X] != '~' && !HitLocations.Contains(new Location(location.X, i)))
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
                sizes[2]++;
                if (sizes[2] == OpponentRemainShips.Max())
                    break;
            }
            if (sizes[2] >= OpponentRemainShips.Min() && HitLocations.Contains(new Location(location.X, location.Y + 1)))
                PossibleShipLocations.Add(new Location(location.X, location.Y + 1));
            for (int j = location.X - 1; ; j--)
            {
                try
                {
                    if (targetBoard[location.Y, j] != '~' && !HitLocations.Contains(new Location(j, location.Y)))
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
                sizes[3]++;
                if (sizes[3] == OpponentRemainShips.Max())
                    break;
            }
            if (sizes[3] >= OpponentRemainShips.Min() && HitLocations.Contains(new Location(location.X - 1, location.Y)))
                PossibleShipLocations.Add(new Location(location.X - 1, location.Y));
        }
        void UpdatePossibleShipLocations() 
        {
            Location locationMin, locationMax;
            if (ShipHits[0].X == ShipHits[1].X)
            {
                int minY = 9, maxY = 0;
                int x = ShipHits[0].X;
                foreach (Location item in ShipHits)
                {
                    if (item.Y - 1 < minY)
                    {
                        minY = item.Y - 1;
                    }
                    if (item.Y + 1 > maxY)
                    {
                        maxY = item.Y + 1;
                    }
                }
                locationMin = new Location(x, minY);
                locationMax = new Location(x, maxY);
                if (HitLocations.Contains(locationMin))
                    PossibleShipLocations.Add(locationMin);
                else if (!ShipHitBorders.Contains(locationMin))
                    ShipHitBorders.Add(locationMin);

                if (HitLocations.Contains(locationMax))
                    PossibleShipLocations.Add(locationMax);
                else if (!ShipHitBorders.Contains(locationMax))
                    ShipHitBorders.Add(locationMax);
            }
            else if (ShipHits[0].Y == ShipHits[1].Y)
            {
                int minX = 9, maxX = 0;
                int y = ShipHits[0].Y;
                foreach (Location item in ShipHits)
                {
                    if (item.X - 1 < minX)
                    {
                        minX = item.X - 1;
                    }
                    if (item.X + 1 > maxX)
                    {
                        maxX = item.X + 1;
                    }
                }
                locationMin = new Location(minX, y);
                locationMax = new Location(maxX, y);
                if (HitLocations.Contains(locationMin))
                    PossibleShipLocations.Add(locationMin);
                else if (!ShipHitBorders.Contains(locationMin))
                    ShipHitBorders.Add(locationMin);

                if (HitLocations.Contains(locationMax))
                    PossibleShipLocations.Add(locationMax);
                else if (!ShipHitBorders.Contains(locationMax))
                    ShipHitBorders.Add(locationMax);
            }
        }
        void RemoveImpossibleFromHitLocations()
        {
            foreach (Location location in ShipHits)
            {
                for (int i = location.Y - 1; i <= location.Y + 1; i++)
                {
                    for (int j = location.X - 1; j <= location.X + 1; j++)
                    {
                        HitLocations.Remove(new Location(j, i));
                    }
                }
            }
        }
    }
}
