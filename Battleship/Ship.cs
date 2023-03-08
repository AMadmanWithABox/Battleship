namespace Battleship;

public class Ship
{
    public int Length { get; set; }
    private int Health { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsVertical { get; set; }
    public bool IsSunk => Health == 0;
    
    public Ship(int length, int x, int y, bool isVertical)
    {
        Length = length;
        Health = length;
        if( length == 1)
        {
            Health = 3;
            length = 3;
        }
        X = x;
        Y = y;
        IsVertical = isVertical;
    }

    public void Hit()
    {
        Health--;
    }
}