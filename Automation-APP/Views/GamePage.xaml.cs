using System;
using Automation_APP.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Automation_APP.Views;

public partial class GamePage : ContentPage
{
    const float CellSize = 24f;
    readonly Services.GameLoopService gameLoop = new();

    GameViewModel ViewModel => (GameViewModel)BindingContext;

    public GamePage()
    {
        InitializeComponent();
        gameLoop.Tick += OnGameLoopTick;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        gameLoop.Start((Microsoft.Maui.Dispatching.IDispatcher)Dispatcher, TimeSpan.FromMilliseconds(16));
    }

    protected override void OnDisappearing()
    {
        gameLoop.Stop();
        base.OnDisappearing();
    }

    void OnGameLoopTick(object? sender, EventArgs e)
    {
        if (!ViewModel.IsGameOver)
        {
            ViewModel.Update();
            gameLoop.Interval = ViewModel.TickInterval;
        }

        GameCanvas.InvalidateSurface();
    }

    void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(new SKColor(7, 17, 31));
        var density = (float)DeviceDisplay.MainDisplayInfo.Density;
        var logicalWidth = e.Info.Width / density;
        var logicalHeight = e.Info.Height / density;
        ViewModel.ResizeGrid(logicalWidth, logicalHeight, CellSize);
        canvas.Scale(density);

        DrawGrid(canvas, logicalWidth, logicalHeight);
        DrawCell(canvas, ViewModel.Fruit, new SKColor(255, 107, 107));
        foreach (var obstacle in ViewModel.Obstacles)
        {
            DrawCell(canvas, obstacle, new SKColor(107, 114, 128));
        }

        if (ViewModel.ActivePowerUp is not null)
        {
            using var glow = new SKPaint { Color = new SKColor(79, 209, 197), MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 8), IsAntialias = true };
            var rect = CellRect(ViewModel.ActivePowerUp.Position);
            canvas.DrawOval(rect, glow);
            DrawCell(canvas, ViewModel.ActivePowerUp.Position, new SKColor(79, 209, 197));
        }

        for (var index = 0; index < ViewModel.Snake.Count; index++)
        {
            DrawCell(canvas, ViewModel.Snake[index], index == 0 ? new SKColor(141, 247, 167) : new SKColor(42, 217, 101));
        }
    }

    static void DrawGrid(SKCanvas canvas, float width, float height)
    {
        using var paint = new SKPaint { Color = new SKColor(255, 255, 255, 18), StrokeWidth = 1 };
        for (float x = 0; x < width; x += CellSize)
        {
            canvas.DrawLine(x, 0, x, height, paint);
        }

        for (float y = 0; y < height; y += CellSize)
        {
            canvas.DrawLine(0, y, width, y, paint);
        }
    }

    static SKRect CellRect(Microsoft.Maui.Graphics.PointF point) => new(point.X * CellSize + 2, point.Y * CellSize + 2, (point.X + 1) * CellSize - 2, (point.Y + 1) * CellSize - 2);

    static void DrawCell(SKCanvas canvas, Microsoft.Maui.Graphics.PointF point, SKColor color)
    {
        using var paint = new SKPaint { Color = color, IsAntialias = true };
        canvas.DrawRoundRect(CellRect(point), 6, 6, paint);
    }

    void OnCanvasTouch(object sender, SKTouchEventArgs e)
    {
        if (e.ActionType != SKTouchAction.Released)
        {
            return;
        }

        var centerX = GameCanvas.CanvasSize.Width / 2f;
        var centerY = GameCanvas.CanvasSize.Height / 2f;
        var deltaX = e.Location.X - centerX;
        var deltaY = e.Location.Y - centerY;
        ViewModel.ChangeDirectionCommand.Execute(Math.Abs(deltaX) > Math.Abs(deltaY) ? deltaX > 0 ? "Right" : "Left" : deltaY > 0 ? "Down" : "Up");
        e.Handled = true;
    }
}
