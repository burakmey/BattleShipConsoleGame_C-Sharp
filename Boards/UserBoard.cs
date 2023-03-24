﻿using System.Drawing;

namespace BattleShipConsoleGame.Boards
{
    internal class UserBoard : Board
    {
        string name = "Buraks Board";
        int maxNameSize = 23;
        public UserBoard()
        {
            CreateEmptyBoard();
        }
        public override void CreateEmptyBoard()
        {
            GameBoard = new char[ROWANDCOLUMN, ROWANDCOLUMN];
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    GameBoard[i, j] = '~';

            OpponentRemainShips = new List<int>();
        }
        public override void PrintBoard()
        {
            Console.WriteLine("_______Your Board________");
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
                    Console.WriteLine("Make sure you entered correct location.");
                }
            }
            while (pass);
            h = location.X - 1;
            v = (ROWANDCOLUMN - 1) - (location.Y - 1);
            if (!((isVerticalPlacement && v >= 0 && v <= ROWANDCOLUMN - shipSize && h >= 0 && h <= 9) || (!isVerticalPlacement && h >= 0 && h <= ROWANDCOLUMN - shipSize && v >= 0 && v <= 9)))
            {
                Console.WriteLine("Make sure you entered valid location");
                goto Start;
            }
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
        }
        public override bool ShootTarget(char[,] targetBoard)
        {
            Location location = GetTargetLocation(targetBoard);
            if (targetBoard[location.Y, location.X] == '%')
            {
                targetBoard[location.Y, location.X] = 'X';
                return true;
            }
            else if (targetBoard[location.Y, location.X] == '~')
            {
                targetBoard[location.Y, location.X] = 'M';
                return false;
            }
            return false;
        }
        public override Location GetTargetLocation(char[,] targetBoard)
        {
            throw new NotImplementedException();
        }
    }
}
