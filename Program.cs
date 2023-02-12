namespace BattleShipConsoleGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board computerBoard = new Board(true);
            Board userBoard = new Board(false);
            PlayGame playGame = new PlayGame(userBoard, computerBoard);
            playGame.PrintGame();
        }
    }
}