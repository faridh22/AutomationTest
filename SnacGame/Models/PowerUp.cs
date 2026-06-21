using SkiaSharp;

namespace SnacGame.Models;

public enum PowerUpType
{
    SlowMotion
}

public class PowerUp
{
    public SKPoint Position { get; set; }
    public PowerUpType Type { get; set; }
}
