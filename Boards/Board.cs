using System.Collections;
using System.Drawing;

namespace BattleShipConsoleGame.Boards
{
    internal abstract class Board
    {
        protected const int ROWANDCOLUMN = 10;
        char[,] gameBoard;
        List<int> opponentRemainShips;
        List<int> placementShips;
        public char[,] GameBoard
        {
            get { return gameBoard; }
            protected set { gameBoard = value; }
        }
        public List<int> OpponentRemainShips
        {
            get { return opponentRemainShips; }
            protected set { opponentRemainShips = value; }
        }
        public List<int> PlacementShips
        {
            get { return placementShips; }
            protected set { placementShips = value; }
        }
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
        public abstract void CreateEmptyBoard();
        public abstract void PrintBoard();
        protected abstract void PlaceShips();
        protected abstract Location GetPlacementLocation();
        public abstract bool ShootTarget(char[,] targetBoard);
        public abstract Location GetTargetLocation(char[,] targetBoard);
        protected enum Ships
        {
            Carrier = 5,
            Battleship = 4,
            Submarine1 = 3,
            Submarine2 = 3,
            Destroyer = 2
        }
    }
}