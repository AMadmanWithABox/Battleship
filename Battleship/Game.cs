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
    private int clock;
    private int _pevTurnX;
    private int _prevTurnY;
    private Player.TileStatus.Status _prevTurnStatus;

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
        AIFire();
        return successfulTurn;
    }

    private void AIFire()
    {
        Random r = new Random();
        int x = r.Next(10);
        int y = r.Next(10);

        if (_prevTurnStatus == Player.TileStatus.Status.Hit)
        {
            try
            {
                while (player.Board[x, y].status == Player.TileStatus.Status.Hit ||
                       player.Board[x, y].status == Player.TileStatus.Status.Miss ||
                       player.Board[x, y].status == Player.TileStatus.Status.Sunk)
                {
                    switch (clock)
                    {
                        case 0:
                            x = _pevTurnX;
                            y = _prevTurnY + 1;
                            break;
                        case 1:
                            x = _pevTurnX;
                            y = _prevTurnY - 1;
                            break;
                        case 2:
                            x = _pevTurnX + 1;
                            y = _prevTurnY;
                            break;
                        case 3:
                            x = _pevTurnX - 1;
                            y = _prevTurnY;
                            break;
                        default:
                            x = r.Next(10);
                            y = r.Next(10);
                            break;
                    }

                    clock++;
                    if(clock > 3) clock = 0;
                }
            }
            catch (IndexOutOfRangeException)
            {
                clock++;
                x = r.Next(10);
                y = r.Next(10);
            }
        }
        else
        {
            while (player.Board[x, y].status == Player.TileStatus.Status.Hit ||
                   player.Board[x, y].status == Player.TileStatus.Status.Miss ||
                   player.Board[x, y].status == Player.TileStatus.Status.Sunk)
            {
                x = r.Next(10);
                y = r.Next(10);
            }
        }

        if (player.Board[x, y].status == Player.TileStatus.Status.Ship)
        {
            computer.Hits++;
            if (_prevTurnStatus == Player.TileStatus.Status.Hit) clock--;
            _prevTurnStatus = Player.TileStatus.Status.Hit;
            player.Board[x, y].status = Player.TileStatus.Status.Hit;
            player.Board[x, y].ship.Hit();
            if (player.Board[x, y].ship.IsSunk)
            {
                int X = player.Board[x, y].ship.X;
                int Y = player.Board[x, y].ship.Y;
                _prevTurnStatus = Player.TileStatus.Status.Sunk;
                
                for (int i = 0; i < player.Board[X, Y].ship.Length; i++)
                {
                    if (player.Board[X, Y].ship.IsVertical)
                    {
                        player.Board[X, Y + i].status = Player.TileStatus.Status.Sunk;
                    }
                    else
                    {
                        player.Board[X + i, Y].status = Player.TileStatus.Status.Sunk;
                    }
                }
            }
        }
        else if(player.Board[x, y].status == Player.TileStatus.Status.Empty)
        {
            player.Board[x, y].status = Player.TileStatus.Status.Miss;
            _prevTurnStatus = Player.TileStatus.Status.Miss;
        }
    }

    public bool CheckForWin(Player p)
    {
        foreach (var ship in p.Ships)
        {
            if (!ship.IsSunk)
            {
                return false;
            }
        }

        return true;
    }
}