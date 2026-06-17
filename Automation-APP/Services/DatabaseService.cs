using Automation_APP.Models;
using SQLite;

namespace Automation_APP.Services;

public sealed class DatabaseService
{
    readonly Lazy<SQLiteAsyncConnection> connection;

    public DatabaseService(string? databasePath = null)
    {
        var path = databasePath ?? Path.Combine(FileSystem.AppDataDirectory, "snac-game.db3");
        connection = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(path));
    }

    public SQLiteAsyncConnection Connection => connection.Value;

    public async Task InitializeAsync()
    {
        await Connection.CreateTableAsync<PlayerProfile>();
        await Connection.CreateTableAsync<GameSession>();
    }
}
