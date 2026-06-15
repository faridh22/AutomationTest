using Microsoft.Maui.Graphics;

namespace Automation_APP.Models;

public enum PowerUpType
{
    SlowMotionBerry
}

public sealed class PowerUp
{
    public PowerUp(PowerUpType type, PointF position, TimeSpan duration)
    {
        Type = type;
        Position = position;
        Duration = duration;
    }

    public PowerUpType Type { get; }
    public PointF Position { get; }
    public TimeSpan Duration { get; }
    public DateTimeOffset SpawnedAt { get; } = DateTimeOffset.UtcNow;
}
