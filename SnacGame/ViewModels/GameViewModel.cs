using CommunityToolkit.Mvvm.Messaging;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SnacGame.Messages;
using SnacGame.Services;

namespace SnacGame.ViewModels;

public class GameViewModel : IRecipient<GameOverMessage>
{
    private const float CellSize = 20f;
    private readonly DifficultyService _difficultyService;
    private readonly PhysicsEngine _physicsEngine;
    private readonly SpawnerService _spawnerService;
    private readonly GameLoopService _gameLoopService;

    public bool IsGameOver { get; private set; }
    public int Score => _physicsEngine.Score;

    public GameViewModel(
        PhysicsEngine physicsEngine,
        DifficultyService difficultyService,
        SpawnerService spawnerService,
        GameLoopService gameLoopService)
    {
        _physicsEngine = physicsEngine;
        _difficultyService = difficultyService;
        _spawnerService = spawnerService;
        _gameLoopService = gameLoopService;

        WeakReferenceMessenger.Default.Register(this);
    }

    public void OnGameTick()
    {
        if (IsGameOver)
        {
            return;
        }

        _physicsEngine.Update();
        _gameLoopService.SetInterval(_difficultyService.GetTickInterval(Score));
    }

    public void OnPaintSurface(SKCanvasView canvasView, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(new SKColor(8, 16, 32));

        _physicsEngine.InitializeGrid(e.Info.Width, e.Info.Height, CellSize);
        _spawnerService.Initialize(e.Info.Width, e.Info.Height, CellSize);

        DrawFood(canvas);
        DrawSnake(canvas);
        DrawScore(canvas);

        if (IsGameOver)
        {
            DrawGameOver(canvas, e.Info.Width, e.Info.Height);
        }
    }

    public void SetDirection(Direction direction) => _physicsEngine.SetDirection(direction);

    public void Restart()
    {
        _physicsEngine.Reset();
        IsGameOver = false;
        _gameLoopService.Start();
    }

    public void Receive(GameOverMessage message)
    {
        IsGameOver = message.Value;
        _gameLoopService.Stop();
    }

    private void DrawFood(SKCanvas canvas)
    {
        using var paint = new SKPaint { Color = SKColors.Red, IsAntialias = true };
        canvas.DrawCircle(_physicsEngine.FoodPosition, CellSize / 2.5f, paint);
    }

    private void DrawSnake(SKCanvas canvas)
    {
        using var headPaint = new SKPaint { Color = SKColors.LimeGreen, IsAntialias = true };
        using var bodyPaint = new SKPaint { Color = SKColors.ForestGreen, IsAntialias = true };

        for (var i = 0; i < _physicsEngine.SnakeBody.Count; i++)
        {
            var point = _physicsEngine.SnakeBody[i];
            var rect = new SKRect(point.X, point.Y, point.X + CellSize, point.Y + CellSize);
            canvas.DrawRoundRect(rect, 4, 4, i == 0 ? headPaint : bodyPaint);
        }
    }

    private void DrawScore(SKCanvas canvas)
    {
        using var paint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 36,
            IsAntialias = true
        };

        canvas.DrawText($"Score: {Score}", 20, 45, paint);
    }

    private static void DrawGameOver(SKCanvas canvas, int width, int height)
    {
        using var overlayPaint = new SKPaint { Color = new SKColor(0, 0, 0, 180) };
        canvas.DrawRect(new SKRect(0, 0, width, height), overlayPaint);

        using var textPaint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 48,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };

        canvas.DrawText("Game Over", width / 2f, height / 2f, textPaint);
    }
}
