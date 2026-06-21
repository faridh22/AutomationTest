using SQLite;

namespace SnacGame.Models;

public class PlayerProfile
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalGames { get; set; }
}
