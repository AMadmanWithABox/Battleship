using System;

namespace Battleship;

public class Game
{
    public const int ShipTiles = 17;
    public bool isPlayerTurn { get; set; }
    public Player player { get; set; }
    public Player computer { get; set; }
    public Phase phase { get; set; }
    public int turns { get; set; }

    public Game()
    {
        isPlayerTurn = true;
        player = new Player(false, this);
        computer = new Player(true, this);
        computer.PlaceShips();
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

    public bool Fire(int x, int y)
    {
        bool successfulTurn = false;
        
        if (computer.Board[x, y].status == Player.TileStatus.Status.Ship)
        {
            successfulTurn = true;
            player.Hits++;
            turns++;
            computer.Board[x, y].status = Player.TileStatus.Status.Hit;
            computer.Board[x, y].ship.Hit();
            if (computer.Board[x, y].ship.IsSunk)
            {
                int X = computer.Board[x, y].ship.X;
                int Y = computer.Board[x, y].ship.Y;
                
                for (int i = 0; i < computer.Board[X, Y].ship.Length; i++)
                {
                    if (computer.Board[X, Y].ship.IsVertical)
                    {
                        computer.Board[X, Y + i].status = Player.TileStatus.Status.Sunk;
                    }
                    else
                    {
                        computer.Board[X + i, Y].status = Player.TileStatus.Status.Sunk;
                    }
                }
            }
        }
        else if(computer.Board[x, y].status == Player.TileStatus.Status.Empty)
        {
            turns++;
            computer.Board[x, y].status = Player.TileStatus.Status.Miss;
        }
        else if (computer.Board[x, y].status == Player.TileStatus.Status.Hit)
        {
            successfulTurn = false;
        }
        else if (computer.Board[x, y].status == Player.TileStatus.Status.Miss)
        {
            successfulTurn = false;
        }
        else if (computer.Board[x, y].status == Player.TileStatus.Status.Sunk)
        {
            successfulTurn = false;
        }
        return successfulTurn;
        
    }

    public bool CheckForWin(Player p)
    {
        if (p.Hits == ShipTiles)
        {
            return true;
        }

        return false;
    }
}