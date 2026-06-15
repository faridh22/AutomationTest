using SQLite;

namespace Automation_APP.Models;

public sealed class GameSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int PlayerId { get; set; }

    public int Score { get; set; }

    public double DurationSeconds { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
