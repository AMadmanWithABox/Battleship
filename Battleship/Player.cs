﻿using System;

namespace Battleship;

public class Player
{
    private bool _isComputer;
    public int RemainingShips { get; set; }
    public int ShipsToPlace { get; set; }
    public TileStatus[,] Board { get; set; }
    public Ship[] Ships { get; set; }

    public Player(bool isComputer)
    {
        RemainingShips = 5;
        ShipsToPlace = 5;
        Board = new TileStatus[10, 10];
        Ships = new Ship[5];
        _isComputer = isComputer;
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
        else PlayerPlaceShips();
    }

    private void PlayerPlaceShips()
    {
        ShipsToPlace--;
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
            Ships[ShipsToPlace] = new Ship(ShipsToPlace, x, y, isVertical);
            for (int i = 0; i < shipLength; i++)
            {
                if (isVertical) Board[x, y + i].ship = Ships[ShipsToPlace];
                else Board[x + i, y].ship = Ships[ShipsToPlace];
            }
        }
        ShipsToPlace--;
        return true;
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