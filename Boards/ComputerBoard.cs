using System.Drawing;

namespace BattleShipConsoleGame.Boards
{
    internal class ComputerBoard : Board
    {
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
        public ComputerBoard(string name = "Computer")
        {
            base.name = name;
            CreateEmptyBoard();
            PlaceShips(Ships.Carrier.ToString(), (int)Ships.Carrier);
            PlaceShips(Ships.Battleship.ToString(), (int)Ships.Battleship);
            PlaceShips(Ships.Submarine1.ToString(), (int)Ships.Submarine1);
            PlaceShips(Ships.Submarine2.ToString(), (int)Ships.Submarine2);
            PlaceShips(Ships.Destroyer.ToString(), (int)Ships.Destroyer);
        }
        public override void CreateEmptyBoard()
        {
            base.CreateEmptyBoard();
            PossibleShipLocations = new List<Location>();
            ShipHits = new List<Location>();
            ShipHitBorders = new List<Location>();
            HitLocations = new List<Location>();
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    HitLocations.Add(new Location(i, j));
        }
        protected override void PlaceShips(string shipName, int shipSize)
        {
            int v, h;
            Random random = new Random();
            Location location;
            bool isVerticalPlacement;
        Start:
            isVerticalPlacement = random.Next(2) == 0 ? true : false;
            if (isVerticalPlacement)
            {
                location = new Location(random.Next(ROWANDCOLUMN), random.Next(ROWANDCOLUMN - shipSize + 1));
            }
            else
            {
                location = new Location(random.Next(ROWANDCOLUMN - shipSize + 1), random.Next(ROWANDCOLUMN));
            }
            h = location.X;
            v = location.Y;
            if (isVerticalPlacement) //It will be vertical placement.
            {
                for (int i = v - 1; i < v + shipSize + 1; i++)
                    for (int j = h - 1; j < h + 2; j++)
                    {
                        try
                        {
                            if (GameBoard[i, j] == '%') //Controlling if it is convenient.
                                goto Start;
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
                                goto Start;
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
        public override bool ShootTarget(char[,] targetBoard)
        {
            if (!isFinished)
            {
                Location location = GetTargetLocation(targetBoard);
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
            if (ShipHits.Count == 0) //Select random hit location and control for possible ship locations.
            {
                index = random.Next(HitLocations.Count);
                location = HitLocations[index];
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
            else if (ShipHits.Count > 0 && ShipHitBorders.Count != 2) //Select random possible ship location and update that possible ship locations.
            {
                index = random.Next(PossibleShipLocations.Count);
                location = PossibleShipLocations[index];
                HitLocations.Remove(location);
                if (targetBoard[location.Y, location.X] == '%')
                {
                    ShipHits.Add(location);
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
            return location = new Location(-1, -1);
        }
        private protected int ControlPossibleHitLocations(Location location, char[,] targetBoard, int number = 4)
        {
            List<int> sizes = new List<int> { 1, 1, 1, 1 };
            if (!HitLocations.Contains(location))
            {
                return 0;
            }
            switch (number) //Calculate max ship lenght on left(0), top(1), right(2) and bottom(3) for specified location.
            {
                case 0:
                    for (int j = location.X - 1; ; j--)
                    {
                        try
                        {
                            if (!HitLocations.Contains(new Location(j, location.Y)))
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        sizes[number]++;
                        if (sizes[number] + ShipHits.Count >= OpponentRemainShips.Max())
                            break;
                    }
                    Console.WriteLine($"sizes[{number}] = {sizes[number]}");
                    return sizes[number];
                case 1:
                    for (int i = location.Y - 1; ; i--)
                    {
                        try
                        {
                            if (!HitLocations.Contains(new Location(location.X, i)))
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        sizes[number]++;
                        if (sizes[number] + ShipHits.Count >= OpponentRemainShips.Max())
                            break;
                    }
                    Console.WriteLine($"sizes[{number}] = {sizes[number]}");
                    return sizes[number];
                case 2:
                    for (int j = location.X + 1; ; j++)
                    {
                        try
                        {
                            if (!HitLocations.Contains(new Location(j, location.Y)))
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        sizes[number]++;
                        if (sizes[number] + ShipHits.Count >= OpponentRemainShips.Max())
                            break;
                    }
                    Console.WriteLine($"sizes[{number}] = {sizes[number]}");
                    return sizes[number];
                case 3:
                    for (int i = location.Y + 1; ; i++)
                    {
                        try
                        {
                            if (!HitLocations.Contains(new Location(location.X, i)))
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        sizes[number]++;
                        if (sizes[number] + ShipHits.Count >= OpponentRemainShips.Max())
                            break;
                    }
                    Console.WriteLine($"sizes[{number}] = {sizes[number]}");
                    return sizes[number];
                default: //Control if there could be ship on specified location. If there is, add surroundings of specified location to possible ship locations.
                    if (ControlPossibleHitLocations(location, targetBoard, 0) + ControlPossibleHitLocations(location, targetBoard, 2) - 1 >= OpponentRemainShips.Min())
                    {
                        if (HitLocations.Contains(new Location(location.X - 1, location.Y)))
                            PossibleShipLocations.Add(new Location(location.X - 1, location.Y));
                        if (HitLocations.Contains(new Location(location.X + 1, location.Y)))
                            PossibleShipLocations.Add(new Location(location.X + 1, location.Y));
                    }
                    if (ControlPossibleHitLocations(location, targetBoard, 1) + ControlPossibleHitLocations(location, targetBoard, 3) - 1 >= OpponentRemainShips.Min())
                    {
                        if (HitLocations.Contains(new Location(location.X, location.Y - 1)))
                            PossibleShipLocations.Add(new Location(location.X, location.Y - 1));
                        if (HitLocations.Contains(new Location(location.X, location.Y + 1)))
                            PossibleShipLocations.Add(new Location(location.X, location.Y + 1));
                    }
                    if (PossibleShipLocations.Count == 0)
                        return -1;
                    return PossibleShipLocations.Count;
            }
        }
        private protected void UpdatePossibleShipLocations(char[,] targetBoard)
        {
            Location locationMin, locationMax;
            bool pass = false;
            ShipHitBorders.Clear();
            PossibleShipLocations.Clear();
            if (ShipHits[0].X == ShipHits[1].X) //Control for horizontal.
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
                foreach (int item in OpponentRemainShips) //Control if there is proper ship for current possible ship locations.
                {
                    if (ShipHits.Count + ControlPossibleHitLocations(locationMin, targetBoard, 1) + ControlPossibleHitLocations(locationMax, targetBoard, 3) >= item
                        && item > ShipHits.Count)
                    {
                        pass = true;
                        break;
                    }
                }
                if (pass && OpponentRemainShips.Max() > ShipHits.Count)
                {
                    if (HitLocations.Contains(locationMin))
                        PossibleShipLocations.Add(locationMin);
                    else
                        ShipHitBorders.Add(locationMin);

                    if (HitLocations.Contains(locationMax))
                        PossibleShipLocations.Add(locationMax);
                    else
                        ShipHitBorders.Add(locationMax);
                }
                else
                {
                    ShipHitBorders.Add(locationMin);
                    ShipHitBorders.Add(locationMax);
                }
            }
            else if (ShipHits[0].Y == ShipHits[1].Y) //Control for vertical.
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
                foreach (int item in OpponentRemainShips) //Control if there is proper ship for current possible ship locations.
                {
                    if (ShipHits.Count + ControlPossibleHitLocations(locationMin, targetBoard, 0) + ControlPossibleHitLocations(locationMax, targetBoard, 2) >= item
                        && item > ShipHits.Count)
                    {
                        pass = true;
                        break;
                    }
                }
                if (pass && OpponentRemainShips.Max() > ShipHits.Count)
                {
                    if (HitLocations.Contains(locationMin))
                        PossibleShipLocations.Add(locationMin);
                    else
                        ShipHitBorders.Add(locationMin);

                    if (HitLocations.Contains(locationMax))
                        PossibleShipLocations.Add(locationMax);
                    else
                        ShipHitBorders.Add(locationMax);
                }
                else
                {
                    ShipHitBorders.Add(locationMin);
                    ShipHitBorders.Add(locationMax);
                }
            }
        }
        private protected void RemoveImpossibleFromHitLocations()
        {
            int removed = 0;
            foreach (Location location in ShipHits)
            {
                for (int i = location.Y - 1; i <= location.Y + 1; i++)
                {
                    for (int j = location.X - 1; j <= location.X + 1; j++)
                    {
                        if (HitLocations.Remove(new Location(j, i)))
                        {
                            removed++;
                        }
                    }
                }
            }
            Console.WriteLine($"removed = {removed}");
        }
    }
}
