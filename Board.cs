using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame
{
    internal class Board
    {
        const int ROWANDCOLUMN = 10;
        char[,] gameBoard;
        bool[,] isBoardHit;
        bool isComputer;
        Queue ships;
        public bool[,] IsBoardHit
        {
            get { return isBoardHit; }
            set { isBoardHit = isBoardHit == null ? value : isBoardHit; }
        }
        public char[,] GameBoard
        {
            get { return gameBoard; }
            set { gameBoard = gameBoard == null ? value : gameBoard; }
        }
        public Board(bool isComputer)
        {
            gameBoard = new char[ROWANDCOLUMN, ROWANDCOLUMN];
            CreateEmptyBoard();
            this.isComputer = isComputer;
            ships = new Queue();
            ships.Enqueue(Ships.Carrier);
            ships.Enqueue(Ships.Battleship);
            ships.Enqueue(Ships.Submarine);
            ships.Enqueue(Ships.Submarine);
            ships.Enqueue(Ships.Destroyer);
            if (!isComputer)
                PrintBoard();
            PlaceShips();
            if(!isComputer)
                PrintBoard();
            PlaceShips();
            if (!isComputer)
                PrintBoard();
            PlaceShips();
            if (!isComputer)
                PrintBoard();
            PlaceShips();
            if (!isComputer)
                PrintBoard();
            PlaceShips();
            if (!isComputer)
                PrintBoard();
            isBoardHit = new bool[ROWANDCOLUMN, ROWANDCOLUMN];
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    IsBoardHit[i, j] = false;
        }
        void CreateEmptyBoard()
        {
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    GameBoard[i, j] = '~';
        }
        void PlaceShips()
        {
            int v = 0, h = 0;
            bool pass = false, isVerticalPlacement = false;
            int ship = (int)ships.Dequeue();
        Start:
            do
            {
                pass = false;
                if (isComputer) //if isComputer = true the placement of ships will be random
                {
                    Random random = new Random();
                    isVerticalPlacement = random.Next(2) == 0 ? true : false;
                    if (isVerticalPlacement)
                    {
                        v = random.Next(ROWANDCOLUMN - ship + 1);
                        h = random.Next(ROWANDCOLUMN);
                    }
                    else
                    {
                        v = random.Next(ROWANDCOLUMN);
                        h = random.Next(ROWANDCOLUMN - ship + 1);
                    }
                }
                else //if isComputer = false the placement of ships will be users opinion
                {
                    do
                    {
                        char input;
                        Console.Write("Determine the placement, vertical(V/v) or horizontal(H/h) : ");
                        input = Console.ReadLine()[0];
                        if (input == 'V' || input == 'v')
                        {
                            isVerticalPlacement = true;
                            break;
                        }
                        else if (input == 'H' || input == 'h')
                        {
                            isVerticalPlacement = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Make sure you entered correct character. V/v or H/h");
                        }
                    }
                    while (true);
                    Point location = GetLocationFromUser();
                    h = location.X - 1;
                    v = (ROWANDCOLUMN - 1) - (location.Y - 1);
                    if (isVerticalPlacement && v >= 0 && v <= ROWANDCOLUMN - ship && h >= 0 && h <= 9)
                    {
                        //doğru
                    }
                    else if(!isVerticalPlacement && h >= 0 && h <= ROWANDCOLUMN - ship && v >= 0 && v <= 9)
                    {
                        //doğru
                    }
                    else
                    {
                        Console.WriteLine("Make sure you entered valid location");
                        goto Start;
                    }
                }
                if (isVerticalPlacement) //if isVerticalPlacement = true it will be vertical placement
                {
                    for (int i = v - 1; i < v + ship + 1; i++)
                        for (int j = h - 1; j < h + 2; j++)
                        {
                            try
                            {
                                if (GameBoard[i, j] == '%') //controlling if it is convenient
                                {
                                    pass = true;
                                    if (!isComputer)
                                        Console.WriteLine("Make sure you entered valid location");
                                    goto Start;
                                }
                            }
                            catch (Exception)
                            {
                                //throw;
                            }
                        }
                    if (!pass)
                    {
                        for (int i = 0; i < ship; i++)
                            GameBoard[v + i, h] = '%';
                    }
                }
                else //if isVerticalPlacement = false it will be horizontal placement
                {
                    for (int i = v - 1; i < v + 2; i++)
                        for (int j = h - 1; j < h + ship + 1; j++)
                        {
                            try
                            {
                                if (GameBoard[i, j] == '%') //controlling if it is convenient
                                {
                                    pass = true;
                                    if (!isComputer)
                                        Console.WriteLine("Make sure you entered valid location");
                                    goto Start;
                                }
                            }
                            catch (Exception)
                            {
                                //throw;
                            }
                        }
                    if (!pass)
                    {
                        for (int i = 0; i < ship; i++)
                            GameBoard[v, h + i] = '%';
                    }
                }
            }
            while (pass);
        }
        public void PrintBoard()
        {
            Console.WriteLine("________Your Board_______");
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
        Point GetLocationFromUser()
        {
            Point location = new Point();
            string input;
            bool pass = true;
            int x, y;
            do
            {
                Console.Write("Input location : ");
                input = Console.ReadLine();
                string[] result = input.Split(",");
                if(result.Length == 2 && int.TryParse(result[0], out x) && int.TryParse(result[1], out y))
                {
                    location.X = x;
                    location.Y = y;
                    pass = false;
                }
                else
                {
                    Console.WriteLine("Make sure you entered correct location.");
                }
            }
            while (pass);
            return location;
        }
        enum Ships
        {
            Destroyer = 2,
            Submarine = 3,
            Battleship = 4,
            Carrier = 5
        }
    }
}