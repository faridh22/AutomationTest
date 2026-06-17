using Automation_APP.Models;
using Automation_APP.Services;

namespace Automation_APP.Repositories;

public interface IPlayerRepository
{
    Task<PlayerProfile> GetPlayerAsync();
    Task UpdateScoreAsync(int score);
}

public sealed class PlayerRepository : IPlayerRepository
{
    readonly DatabaseService databaseService;

    public PlayerRepository(DatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    public async Task<PlayerProfile> GetPlayerAsync()
    {
        await databaseService.InitializeAsync();
        var player = await databaseService.Connection.Table<PlayerProfile>().FirstOrDefaultAsync();
        if (player is not null)
        {
            return player;
        }

        player = new PlayerProfile();
        await databaseService.Connection.InsertAsync(player);
        return player;
    }

    public async Task UpdateScoreAsync(int score)
    {
        var player = await GetPlayerAsync();
        player.TotalGamesPlayed++;
        player.HighScore = Math.Max(player.HighScore, score);
        await databaseService.Connection.UpdateAsync(player);
    }
}
