using Automation_APP.Models;
using Automation_APP.Services;

namespace Automation_APP.Repositories;

public interface ISessionRepository
{
    Task SaveSessionAsync(GameSession session);
    Task<IReadOnlyList<GameSession>> GetHistoryAsync();
}

public sealed class SessionRepository : ISessionRepository
{
    readonly DatabaseService databaseService;

    public SessionRepository(DatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    public async Task SaveSessionAsync(GameSession session)
    {
        await databaseService.InitializeAsync();
        await databaseService.Connection.InsertAsync(session);
    }

    public async Task<IReadOnlyList<GameSession>> GetHistoryAsync()
    {
        await databaseService.InitializeAsync();
        return await databaseService.Connection.Table<GameSession>().OrderByDescending(session => session.Timestamp).ToListAsync();
    }
}
