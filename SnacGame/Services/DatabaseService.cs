using SQLite;
using SnacGame.Models;

namespace SnacGame.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private readonly string _dbPath;

    public DatabaseService(string dbPath)
    {
        _dbPath = dbPath;
    }

    private async Task Init()
    {
        if (_database is not null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath);

        await _database.CreateTableAsync<PlayerProfile>();
        await _database.CreateTableAsync<GameSession>();
    }

    public async Task<List<PlayerProfile>> GetPlayerProfilesAsync()
    {
        await Init();
        return await _database.Table<PlayerProfile>().ToListAsync();
    }

    public async Task<PlayerProfile?> GetPlayerProfileAsync(int id)
    {
        await Init();
        return await _database.Table<PlayerProfile>().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task SavePlayerProfileAsync(PlayerProfile profile)
    {
        await Init();
        if (profile.Id != 0)
        {
            await _database.UpdateAsync(profile);
        }
        else
        {
            await _database.InsertAsync(profile);
        }
    }

    public async Task<List<GameSession>> GetGameSessionsAsync()
    {
        await Init();
        return await _database.Table<GameSession>().OrderByDescending(s => s.Timestamp).ToListAsync();
    }

    public async Task SaveGameSessionAsync(GameSession session)
    {
        await Init();
        await _database.InsertAsync(session);
    }

    public async Task UpdatePlayerTotalGamesAsync(int playerId)
    {
        await Init();
        var profile = await _database.Table<PlayerProfile>().FirstOrDefaultAsync(p => p.Id == playerId);
        if (profile != null)
        {
            profile.TotalGames++;
            await _database.UpdateAsync(profile);
        }
    }
}
