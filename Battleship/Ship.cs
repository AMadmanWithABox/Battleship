namespace Battleship;

public class Ship
{
    private int Length { get; set; }
    private int Health { get; set; }
    private int X { get; set; }
    private int Y { get; set; }
    private bool IsVertical { get; set; }
    private bool IsSunk => Health == 0;
    
    public Ship(int length, int x, int y, bool isVertical)
    {
        Length = length;
        Health = length;
        X = x;
        Y = y;
        IsVertical = isVertical;
    }

    public void Hit()
    {
        Health--;
    }
}