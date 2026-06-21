namespace SnacGame.Models;

public class SnakeSegment
{
    public System.Numerics.Vector2 Position { get; set; }

    public SnakeSegment(float x, float y)
    {
        Position = new System.Numerics.Vector2(x, y);
    }
}
