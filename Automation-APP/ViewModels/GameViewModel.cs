using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Automation_APP.Models;
using Automation_APP.Repositories;
using Automation_APP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Graphics;

namespace Automation_APP.ViewModels;

public partial class GameViewModel : ObservableObject, IRecipient<GameOverMessage>
{
    readonly PhysicsEngine physicsEngine = new();
    readonly DifficultyService difficultyService = new();
    readonly SpawnerService spawnerService = new();
    readonly PersistenceService persistenceService = new();
    readonly HapticService hapticService = new();
    readonly IPlayerRepository playerRepository;
    readonly ISessionRepository sessionRepository;
    CancellationTokenSource powerUpTokenSource = new();

    public GameViewModel()
    {
        var database = new DatabaseService();
        playerRepository = new PlayerRepository(database);
        sessionRepository = new SessionRepository(database);
        HighScore = persistenceService.GetHighScore();
        WeakReferenceMessenger.Default.Register(this);
    }

    public IReadOnlyList<PointF> Snake => physicsEngine.Snake;
    public IReadOnlyList<PointF> Obstacles => difficultyService.Obstacles;
    public PowerUp? ActivePowerUp => spawnerService.ActivePowerUp;
    public PointF Fruit => physicsEngine.Fruit;
    public int GridWidth => physicsEngine.GridWidth;
    public int GridHeight => physicsEngine.GridHeight;
    public bool SlowMotionActive => spawnerService.SlowMotionActive;

    [ObservableProperty]
    int score;

    [ObservableProperty]
    int highScore;

    [ObservableProperty]
    bool isGameOver;

    public TimeSpan TickInterval => difficultyService.GetTickInterval(Score, SlowMotionActive);

    public void ResizeGrid(float pixelWidth, float pixelHeight, float cellSize) => physicsEngine.SetGridFromCanvas(pixelWidth, pixelHeight, cellSize);

    public void Update()
    {
        var obstacles = difficultyService.UpdateObstacles(Score, GridWidth, GridHeight);
        physicsEngine.Update(obstacles);
        Score = physicsEngine.Score;
        OnPropertyChanged(nameof(Fruit));
        OnPropertyChanged(nameof(ActivePowerUp));

        var powerUp = spawnerService.TrySpawn(Score, GridWidth, GridHeight, Snake.Concat(obstacles).ToArray());
        if (powerUp is not null && Snake[0] == powerUp.Position)
        {
            hapticService.Click();
            _ = spawnerService.ActivateAsync(powerUp, powerUpTokenSource.Token);
        }
    }

    [RelayCommand]
    public void ChangeDirection(string direction)
    {
        physicsEngine.Direction = direction switch
        {
            "Up" when physicsEngine.Direction != SnakeDirection.Down => SnakeDirection.Up,
            "Down" when physicsEngine.Direction != SnakeDirection.Up => SnakeDirection.Down,
            "Left" when physicsEngine.Direction != SnakeDirection.Right => SnakeDirection.Left,
            "Right" when physicsEngine.Direction != SnakeDirection.Left => SnakeDirection.Right,
            _ => physicsEngine.Direction
        };
    }

    [RelayCommand]
    public void Restart()
    {
        powerUpTokenSource.Cancel();
        powerUpTokenSource = new CancellationTokenSource();
        difficultyService.Reset();
        spawnerService.Reset();
        physicsEngine.Reset(GridWidth, GridHeight);
        IsGameOver = false;
        Score = 0;
    }

    public async void Receive(GameOverMessage message)
    {
        IsGameOver = true;
        hapticService.LongPress();
        persistenceService.SaveHighScore(message.Score);
        HighScore = Math.Max(HighScore, message.Score);
        var player = await playerRepository.GetPlayerAsync();
        await playerRepository.UpdateScoreAsync(message.Score);
        await sessionRepository.SaveSessionAsync(new GameSession
        {
            PlayerId = player.Id,
            Score = message.Score,
            DurationSeconds = message.Duration.TotalSeconds,
            Timestamp = DateTime.UtcNow
        });
    }
}
