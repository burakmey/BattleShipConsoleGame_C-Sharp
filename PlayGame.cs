using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame
{
    public readonly struct Location
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; init; }
        public int Y { get; init; }
    }
    internal class PlayGame
    {
        const int ROWANDCOLUMN = 10;
        List<Location> HitLocations;
        List<Location> possibleShipHits;
        Board userBoard;
        Board computerBoard;
        public PlayGame(Board userBoard, Board computerBoard)
        {
            this.userBoard = userBoard;
            this.computerBoard = computerBoard;
            HitLocations = new List<Location>();
            for (int i = 0; i < ROWANDCOLUMN; i++)
            {
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Location location = new Location(i, j);
                    HitLocations.Add(location);
                }
            }
        }
        public void PrintGame()
        {
            Console.WriteLine(
                "________Your Board_______" + "   " + "______Computer Board_____");
            for (int i = 0; i < ROWANDCOLUMN; i++)
            {
                if (i != 0)
                    Console.Write($"|{ROWANDCOLUMN - i} |");
                else
                    Console.Write($"|{ROWANDCOLUMN - i}|");
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Console.Write($"{userBoard.GameBoard[i, j]} ");
                }
                Console.Write("|   ");
                if (i != 0)
                    Console.Write($"|{ROWANDCOLUMN - i} |");
                else
                    Console.Write($"|{ROWANDCOLUMN - i}|");
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Console.Write($"{computerBoard.GameBoard[i, j]} ");
                }
                Console.Write("|\n");
            }
            Console.WriteLine("|yx|1|2|3|4|5|6|7|8|9|10|" + "   " + "|yx|1|2|3|4|5|6|7|8|9|10|" + "\n");
        }
        public bool ShootTarget(bool isComputer)
        {
            if (isComputer)
            {
                Location location = GetLocation();
                if (userBoard.GameBoard[location.Y, location.X] == '%' && possibleShipHits == null)
                {
                    possibleShipHits = new List<Location>();
                    possibleShipHits.Add(location);
                    possibleShipHits.Add(new Location(location.Y, location.X - 1));
                    possibleShipHits.Add(new Location(location.Y - 1, location.X));
                    possibleShipHits.Add(new Location(location.Y, location.X + 1));
                    possibleShipHits.Add(new Location(location.Y + 1, location.X));
                    userBoard.GameBoard[location.Y, location.X] = 'X';
                    return true;
                }
                else if(userBoard.GameBoard[location.Y, location.X] == '%')
                {
                    userBoard.GameBoard[location.Y, location.X] = 'X';
                    return true;
                }
                else if (userBoard.GameBoard[location.Y, location.X] == '~')
                {
                    userBoard.GameBoard[location.Y, location.X] = 'M';
                    return false;
                }
            }
            else
            {

            }
            return false;
        }
        public Location GetLocation()
        {
            Random random = new Random();
            int index;
            if (possibleShipHits != null)
            {
                do
                {
                    index = random.Next(1, possibleShipHits.Count);
                    Location location = possibleShipHits[index];
                    if (location.Y >= 0 && location.Y >= 9 && location.X >= 0 && location.X >= 9)
                    {
                        try
                        {
                            for (int i = location.Y - 1; i < location.Y + 2; i++)
                            {
                                for (int j = location.X - 1; j < location.X + 2; j++)
                                {
                                    if (userBoard.GameBoard[i, j] == 'X' && i != location.Y && j != location.X)
                                    {
                                        possibleShipHits.RemoveAt(index);
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        possibleShipHits.RemoveAt(index);
                    }
                    return location;
                }
                while(true);
            }
            else
            {
                index = random.Next(HitLocations.Count);
                Location location = HitLocations[index];
                HitLocations.RemoveAt(index);
                return location;
            }
            return new Location();
        }
    }
}
