using SQLite;

namespace Automation_APP.Models;

public sealed class PlayerProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(80)]
    public string Name { get; set; } = "Player";

    public int TotalGamesPlayed { get; set; }

    public int HighScore { get; set; }
}
