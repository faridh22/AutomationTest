using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SnacGame.Services;
using SnacGame.ViewModels;
using SnacGame.Repositories;
using SnacGame.Models;

namespace SnacGame;

public static class MauiProgram
{
	public static IServiceProvider Services { get; private set; } = default!;

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.UseSkiaSharp();

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "snakgame.db3");
		builder.Services.AddSingleton<DatabaseService>(s => new DatabaseService(dbPath));
		builder.Services.AddSingleton<PlayerRepository>();
		builder.Services.AddSingleton<SessionRepository>();

		builder.Services.AddSingleton<GameLoopService>();
		builder.Services.AddSingleton<PhysicsEngine>();
		builder.Services.AddSingleton<IHapticService, HapticService>();
		builder.Services.AddSingleton<SpawnerService>();
		builder.Services.AddSingleton<DifficultyService>();

		builder.Services.AddTransient<GameViewModel>();
		builder.Services.AddTransient<GamePage>();

		var app = builder.Build();
		Services = app.Services;
		return app;
	}
}
