namespace BattleShipConsoleGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board computerBoard = new Board(true);
            computerBoard.PrintBoard();
        }
    }
}