using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using CommunityToolkit.Mvvm.Messaging;
using SnacGame.Messages;

namespace SnacGame.Services;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public class PhysicsEngine
{
	private readonly Random _random = new();
	private readonly IHapticService _hapticService;
	public List<SKPoint> SnakeBody { get; private set; } = new();
	public SKPoint FoodPosition { get; private set; }
	public Direction CurrentDirection { get; private set; } = Direction.Right;
	private Direction _nextDirection = Direction.Right;

	private float _cellSize;
	private int _gridWidth;
	private int _gridHeight;

	public int Score { get; private set; } = 0;

	public PhysicsEngine(IHapticService hapticService)
	{
		_hapticService = hapticService;
		SnakeBody.Add(new SKPoint(5 * 20, 5 * 20));
		SnakeBody.Add(new SKPoint(4 * 20, 5 * 20));
		SnakeBody.Add(new SKPoint(3 * 20, 5 * 20));
	}

	public void InitializeGrid(float width, float height, float cellSize)
	{
		_cellSize = cellSize;
		_gridWidth = (int)(width / _cellSize);
		_gridHeight = (int)(height / _cellSize);
		if (FoodPosition.X == 0 && FoodPosition.Y == 0)
		{
			SpawnFood();
		}
	}

	public void SetDirection(Direction direction)
	{
		if ((direction == Direction.Up && CurrentDirection != Direction.Down) ||
			(direction == Direction.Down && CurrentDirection != Direction.Up) ||
			(direction == Direction.Left && CurrentDirection != Direction.Right) ||
			(direction == Direction.Right && CurrentDirection != Direction.Left))
		{
			_nextDirection = direction;
		}
	}

	public void Update()
	{
		CurrentDirection = _nextDirection;
		SKPoint newHead = new SKPoint(SnakeBody.First().X, SnakeBody.First().Y);
		if (CurrentDirection == Direction.Up) newHead = new SKPoint(newHead.X, newHead.Y - _cellSize);
		else if (CurrentDirection == Direction.Down) newHead = new SKPoint(newHead.X, newHead.Y + _cellSize);
		else if (CurrentDirection == Direction.Left) newHead = new SKPoint(newHead.X - _cellSize, newHead.Y);
		else if (CurrentDirection == Direction.Right) newHead = new SKPoint(newHead.X + _cellSize, newHead.Y);

		var willEatFood = newHead.Equals(FoodPosition);
		var collisionBody = willEatFood ? SnakeBody : SnakeBody.Take(SnakeBody.Count - 1);
		if (newHead.X < 0 || newHead.X >= _gridWidth * _cellSize || newHead.Y < 0 || newHead.Y >= _gridHeight * _cellSize || collisionBody.Any(s => s.Equals(newHead)))
		{
			HandleGameOver();
			return;
		}

		SnakeBody.Insert(0, newHead);
		if (willEatFood)
		{
			Score++;
			_hapticService.PlaySuccessFeedback();
			SpawnFood();
		}
		else
		{
			SnakeBody.RemoveAt(SnakeBody.Count - 1);
		}
	}

	private void SpawnFood()
	{
		if (_gridWidth <= 0 || _gridHeight <= 0 || _cellSize <= 0)
		{
			return;
		}

		SKPoint position;
		do
		{
			var x = _random.Next(0, _gridWidth) * (int)_cellSize;
			var y = _random.Next(0, _gridHeight) * (int)_cellSize;
			position = new SKPoint(x, y);
		}
		while (SnakeBody.Any(s => s.Equals(position)));

		FoodPosition = position;
	}

	private void HandleGameOver()
	{
		_hapticService.PlayErrorFeedback();
		WeakReferenceMessenger.Default.Send(new GameOverMessage(true));
	}
	
	public void Reset()
	{
		SnakeBody.Clear();
		SnakeBody.Add(new SKPoint(5 * 20, 5 * 20));
		SnakeBody.Add(new SKPoint(4 * 20, 5 * 20));
		SnakeBody.Add(new SKPoint(3 * 20, 5 * 20));
		Score = 0;
		CurrentDirection = Direction.Right;
		_nextDirection = Direction.Right;
		SpawnFood();
	}
}
