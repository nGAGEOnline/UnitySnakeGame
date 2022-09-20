using System;
using System.Collections.Generic;
using System.Linq;
using SnakeLib.Enums;
using SnakeLib.Helpers;
using SnakeLib.Interfaces;
using UnityEngine;

public class UnitySnakeGameInput : MonoBehaviour, ISnakeGameInput
{
	public Direction Direction { get; private set; } = Direction.Right;

	[SerializeField] private int _bufferSize = 2;

	private readonly Queue<Direction> _directionChanges = new();

	public void Listen()
	{
		if (_directionChanges.Count > 0)
			Direction = _directionChanges.Dequeue();
	}

	private void Update()
	{
		var direction = GetDirectionFromInput();
		TryChangeDirection(direction);
	}
	public void Reset() 
		=> Direction = Direction.Right;

	private Direction GetDirectionFromInput()
	{
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			return Direction.Up;
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			return Direction.Down;
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			return Direction.Left;
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			return Direction.Right;
		
		return Direction;
	}

	private void TryChangeDirection(Direction direction)
	{
		if (CanChangeDirection(direction))
			_directionChanges.Enqueue(direction);
	}

	private bool CanChangeDirection(Direction newDirection)
	{
		if (_directionChanges.Count == _bufferSize || newDirection == Direction || _directionChanges.Contains(newDirection))
			return false;

		var lastDirection = GetLastDirection();
		return newDirection != lastDirection && newDirection != lastDirection.Opposite();
	}

	private Direction GetLastDirection()
		=> _directionChanges.Count == 0
			? Direction
			: _directionChanges.Last();
}
