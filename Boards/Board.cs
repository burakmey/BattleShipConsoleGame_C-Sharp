namespace BattleShipConsoleGame.Boards
{
    internal abstract class Board
    {
        public string name, board = "'s Board";
        public int maxNameSize = 25;
        protected const int ROWANDCOLUMN = 10;
        public bool isFinished = false;
        char[,] gameBoard;
        List<int> opponentRemainShips;
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
        public virtual void CreateEmptyBoard()
        {
            GameBoard = new char[ROWANDCOLUMN, ROWANDCOLUMN];
            for (int i = 0; i < ROWANDCOLUMN; i++)
                for (int j = 0; j < ROWANDCOLUMN; j++)
                    GameBoard[i, j] = '~';

            OpponentRemainShips = new List<int>();
        }
        public virtual void PrintBoard()
        {
            Console.WriteLine($"{name}{board}");
            Console.WriteLine("_________________________");
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
        protected abstract void PlaceShips(string shipName, int shipSize);
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