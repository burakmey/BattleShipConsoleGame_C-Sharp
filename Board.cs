using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsoleGame
{
    internal class Board
    {
        const int ROWANDCOLUMN = 10;
        char[,] gameBoard;
        bool isComputer;
        Queue ships;
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
            PlaceShips();
            PlaceShips();
            PlaceShips();
            PlaceShips();
            PlaceShips();
        }
        void CreateEmptyBoard()
        {
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    GameBoard[i, j] = '~';
        }
        void PlaceShips()
        {
            int v = 0, h = 0, v_, h_, va, vb, ha, hb;
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
                    //Console.WriteLine(isVerticalPlacement);
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

                }
                if (isVerticalPlacement) //if isVerticalPlacement = true it will be vertical placement
                {
                    for (int i = v - 1; i < v + ship + 2; i++)
                        for (int j = h - 1; j < h + 2; j++)
                        {
                            try
                            {
                                if (GameBoard[i, j] == '%') //controlling if it is convenient
                                {
                                    pass = true;
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
                            gameBoard[v + i, h] = '%';
                    }
                }
                else //if isVerticalPlacement = false it will be horizontal placement
                {
                    for (int i = v - 1; i < v + 2; i++)
                        for (int j = h - 1; j < h + ship + 2; j++)
                        {
                            try
                            {
                                if (GameBoard[i, j] == '%') //controlling if it is convenient
                                {
                                    pass = true;
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
                            gameBoard[v, h + i] = '%';
                    }
                }
            }
            while (pass);
        }
        public void PrintBoard()
        {
            for (int i = 0; i < ROWANDCOLUMN; i++)
            {
                for (int j = 0; j < ROWANDCOLUMN; j++)
                {
                    Console.Write($"{GameBoard[i, j]} ");
                }
                Console.Write("\n");
            }
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
