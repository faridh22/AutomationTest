using System;
using System.Collections.Generic;
using SnacGame.Models;
using SkiaSharp;

namespace SnacGame.Services;

public class SpawnerService
{
    private readonly Random _random = new();
    private float _cellSize;
    private int _gridWidth;
    private int _gridHeight;

    public List<PowerUp> ActivePowerUps { get; private set; } = new();

    public SpawnerService()
    {
        _cellSize = 0;
    }

    public void Initialize(float width, float height, float cellSize)
    {
        _cellSize = cellSize;
        _gridWidth = (int)(width / _cellSize);
        _gridHeight = (int)(height / _cellSize);
    }

    public void TrySpawnPowerUp()
    {
        if (_random.NextDouble() < 0.3) // 30% chance to spawn powerup on food consumption
        {
            int x = _random.Next(0, _gridWidth);
            int y = _random.Next(0, _gridHeight);

            ActivePowerUps.Add(new PowerUp
            {
                Position = new SKPoint(x * _cellSize + _cellSize / 2, y * _cellSize + _cellSize / 2),
                Type = PowerUpType.SlowMotion
            });
        }
    }

    public void RemovePowerUp(PowerUp powerUp)
    {
        ActivePowerUps.Remove(powerUp);
    }
}
