using System;
using System.Collections.Generic;
using System.Linq;
using Automation_APP.Models;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Graphics;

namespace Automation_APP.Services;

public enum SnakeDirection
{
    Up,
    Down,
    Left,
    Right
}

public sealed record GameOverMessage(int Score, TimeSpan Duration);

public sealed class PhysicsEngine
{
    readonly List<PointF> snake = new();
    readonly Random random = new();
    DateTimeOffset startedAt;
    PointF fruit;

    public PhysicsEngine()
    {
        Reset();
    }

    public IReadOnlyList<PointF> Snake => snake;
    public IReadOnlyList<PointF> Obstacles { get; private set; } = Array.Empty<PointF>();
    public PointF Fruit => fruit;
    public int Score { get; private set; }
    public int GridWidth { get; private set; } = 24;
    public int GridHeight { get; private set; } = 32;
    public SnakeDirection Direction { get; set; } = SnakeDirection.Right;
    public bool IsGameOver { get; private set; }

    public void Reset(int gridWidth = 24, int gridHeight = 32)
    {
        GridWidth = Math.Max(8, gridWidth);
        GridHeight = Math.Max(8, gridHeight);
        Score = 0;
        IsGameOver = false;
        Direction = SnakeDirection.Right;
        startedAt = DateTimeOffset.UtcNow;
        snake.Clear();
        var center = new PointF(GridWidth / 2, GridHeight / 2);
        snake.Add(center);
        snake.Add(new PointF(center.X - 1, center.Y));
        snake.Add(new PointF(center.X - 2, center.Y));
        Obstacles = Array.Empty<PointF>();
        SpawnFruit();
    }

    public void SetGridFromCanvas(float pixelWidth, float pixelHeight, float cellSize)
    {
        var width = Math.Max(8, (int)MathF.Floor(pixelWidth / cellSize));
        var height = Math.Max(8, (int)MathF.Floor(pixelHeight / cellSize));
        if (width != GridWidth || height != GridHeight)
        {
            Reset(width, height);
        }
    }

    public void Update(IReadOnlyList<PointF>? obstacles = null)
    {
        if (IsGameOver)
        {
            return;
        }

        if (obstacles is not null)
        {
            Obstacles = obstacles;
        }

        var head = snake[0];
        var next = Direction switch
        {
            SnakeDirection.Up => new PointF(head.X, head.Y - 1),
            SnakeDirection.Down => new PointF(head.X, head.Y + 1),
            SnakeDirection.Left => new PointF(head.X - 1, head.Y),
            SnakeDirection.Right => new PointF(head.X + 1, head.Y),
            _ => head
        };

        if (IsBoundaryCollision(next) || IsSelfCollision(next) || Obstacles.Any(point => point == next))
        {
            IsGameOver = true;
            WeakReferenceMessenger.Default.Send(new GameOverMessage(Score, DateTimeOffset.UtcNow - startedAt));
            return;
        }

        snake.Insert(0, next);
        if (next == fruit)
        {
            Score += 10;
            SpawnFruit();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    public bool IsBoundaryCollision(PointF point) => point.X < 0 || point.Y < 0 || point.X >= GridWidth || point.Y >= GridHeight;

    public bool IsSelfCollision(PointF point) => snake.Any(segment => segment == point);

    void SpawnFruit()
    {
        PointF candidate;
        do
        {
            candidate = new PointF(random.Next(GridWidth), random.Next(GridHeight));
        }
        while (snake.Contains(candidate) || Obstacles.Contains(candidate));

        fruit = candidate;
    }
}
