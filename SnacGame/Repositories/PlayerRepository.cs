using SnacGame.Models;
using SnacGame.Services;

namespace SnacGame.Repositories;

public class PlayerRepository
{
    private readonly DatabaseService _dbService;

    public PlayerRepository(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task<List<PlayerProfile>> GetAllPlayersAsync()
    {
        return await _dbService.GetPlayerProfilesAsync();
    }

    public async Task<PlayerProfile?> GetPlayerByIdAsync(int id)
    {
        return await _dbService.GetPlayerProfileAsync(id);
    }

    public async Task SavePlayerAsync(string name)
    {
        var profile = new PlayerProfile { Name = name, TotalGames = 0 };
        await _dbService.SavePlayerProfileAsync(profile);
    }
}
