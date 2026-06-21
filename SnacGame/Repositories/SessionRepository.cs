using SnacGame.Models;
using SnacGame.Services;

namespace SnacGame.Repositories;

public class SessionRepository
{
    private readonly DatabaseService _dbService;

    public SessionRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task<List<GameSession>> GetHighScoresAsync()
    {
        return await _dbService.GetGameSessionsAsync();
    }

    public async Task SaveSessionAsync(int score)
    {
        var session = new GameSession
        {
            Score = score,
            Timestamp = DateTime.Now
        };
        await _dbService.SaveGameSessionAsync(session);
    }
}
