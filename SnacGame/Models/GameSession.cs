using SQLite;

namespace SnacGame.Models;

public class GameSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime Timestamp { get; set; }
}
