using System;

namespace Battleship;

public class Player
{
    private bool _isComputer;
    public int RemainingShips { get; set; }
    public int ShipsToPlace { get; set; }
    public int Hits { get; set; }
    public TileStatus[,] Board { get; set; }
    public Ship[] Ships { get; set; }
    private Game game { set; get; }

    public Player(bool isComputer, Game game)
    {
        RemainingShips = 5;
        ShipsToPlace = 5;
        Board = new TileStatus[10, 10];
        Ships = new Ship[5];
        Hits = 0;
        _isComputer = isComputer;
        this.game = game;
        initBoard();
    }
    
    private void initBoard()
    {
        for (int row = 0; row <= 9; row++)
        {
            for (int col = 0; col <= 9; col++)
            {
                Board[row, col] = new TileStatus();
            }
        }
    }
    
    public void PlaceShips()
    {
        if(ShipsToPlace == 0) return;
        if (_isComputer) AiPlaceShips();
    }
    
    public void PlaceShips(int x , int y, bool isVertical)
    {
        if (ShipsToPlace == 0) return;
        if (!_isComputer) placeShip(x, y, GetShipLength(), isVertical)  ;
    }

   
    private void AiPlaceShips()
    {
        Random r = new Random();
        while (ShipsToPlace != 0)
        {
            int x = r.Next(10);
            int y = r.Next(10);
            bool isVertical = r.Next(2) == 1;
            int length = GetShipLength();
            placeShip(x, y, length, isVertical);
        }
    }

    private int GetShipLength()
    {
        if (ShipsToPlace == 1) return 3;
        return ShipsToPlace;
    }

    private bool placeShip(int x, int y, int shipLength, bool isVertical)
    {
        if (CheckPlacement(x, y, shipLength, isVertical))
        {
            Ships[ShipsToPlace - 1] = new Ship(ShipsToPlace, x, y, isVertical);

            for (int i = 0; i < shipLength; i++)
            {
                if (isVertical)
                {
                    Board[x, y + i].ship = Ships[ShipsToPlace - 1];
                    Board[x, y + i].status = TileStatus.Status.Ship;
                }
                else
                {
                    Board[x + i, y].ship = Ships[ShipsToPlace - 1];
                    Board[x + i, y].status = TileStatus.Status.Ship;
                }
                
            }
            if (--ShipsToPlace == 0 && !_isComputer)
            {
                game.phase = Game.Phase.PlayGame;
            }
            
            return true;
        }

        return false;
    }


    public bool CheckPlacement(int x, int y, int shipLength, bool isVertical)
    {
        try
        {
            for (int i = 0; i < shipLength; i++)
            {
                if (isVertical && Board[x, y + i].status == TileStatus.Status.Empty) continue;
                if (Board[x + i, y].status == TileStatus.Status.Empty) continue;
                return false;
            }
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }

        return true;
    }

    public TileStatus checkWin()
    {
        return null;
    }
    
    public class TileStatus
    {
        public Status status { get; set; }
        public Ship ship { get; set; }
        
        public TileStatus()
        {
            status = Status.Empty;
        }
        
        public enum Status
        {
            Empty,
            Ship,
            Hit,
            Miss,
            Sunk
        }
    }
}