namespace Battleship;

public class Game
{
    public Game()
        {
        }

    //create 8x8 array
        public TileStatus[,] board { get; set; } = new TileStatus[8, 8];

        public bool isPlayerTurn { get; set; } = true;

        public TileStatus checkWin()
        {
            return TileStatus.Empty;
        }

        public void UpdateScore()
        {
            
        }
        
        public enum TileStatus
        {
            Empty,
            Miss,
            Hit
        
        }
}