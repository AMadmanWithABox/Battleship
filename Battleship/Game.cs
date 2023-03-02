namespace Battleship;

public class Game
{
    public bool isPlayerTurn { get; set; }
    public Player player { get; set; }
    public Player computer { get; set; }
    public Phase phase { get; set; }

    public Game()
    {
        isPlayerTurn = true;
        player = new Player(false);
        computer = new Player(true);
        phase = Phase.PlaceShips;
    }

    public bool CheckPlacement(int x, int y, int length, bool isVertical)
    {
        return player.CheckPlacement(x, y, length, isVertical);
    }

    public enum Phase
    {
        PlaceShips,
        PlayGame
    }
}