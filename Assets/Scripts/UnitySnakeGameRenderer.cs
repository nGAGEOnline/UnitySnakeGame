using System;
using System.Collections.Generic;
using System.Linq;
using SnakeLib.Enums;
using SnakeLib.Interfaces;
using SnakeLib.Structs;
using Unity.VisualScripting;
using UnityEngine;

public class UnitySnakeGameRenderer : MonoBehaviour, ISnakeGameRenderer
{
	[Header("Dependency")]
	[SerializeField] private UnitySnakeGame _snakeGame;
	[SerializeField] private Camera _camera;
	
	[Header("Prefabs")]
	[SerializeField] private Transform _snake;
	[SerializeField] private Transform _snakeDead;
	[SerializeField] private Transform _snakeHead;
	[SerializeField] private Transform _snakeHeadDead;
	[SerializeField] private Transform _fruit;
	[SerializeField] private Transform _empty;
	[SerializeField] private Transform _blank;
	[SerializeField] private Transform _border;

	[Header("Other")]
	[SerializeField] private Color _backgroundColor;

	private Transform _gridTransform;
	private Transform _emptyTransform;
	private Transform _borderTransform;
	
	private readonly Dictionary<Coord, Transform> _grid = new();

	private void Awake()
	{
		var screenRatio = Screen.height / (float)Screen.width;
		var ratio = _snakeGame.Settings.Height / (float)_snakeGame.Settings.Width;
		var height = _snakeGame.Settings.Width * ratio;
		var size = height * screenRatio;
		_camera.orthographicSize = size + 1;
		_camera.backgroundColor = _backgroundColor;
		
		foreach (Transform child in transform)
		{
			switch (child.name)
			{
				case "Grid":
					_gridTransform = child;
					break;
				case "Border":
					_borderTransform = child;
					break;
				case "Empty":
					_emptyTransform = child;
					break;
			}
		}
	}

	public void RenderBorder()
	{
		for (var y = -1; y <= _snakeGame.Settings.Height; y++)
			for (var x = -1; x <= _snakeGame.Settings.Width; x++)
				if (x < 0 || x >= _snakeGame.Settings.Width || y < 0 || y >= _snakeGame.Settings.Height)
					Print(new Coord(x, y), _border, _borderTransform);
	}

	public void RenderGrid()
	{
		for (var y = 0; y < _snakeGame.Settings.Height; y++)
			for (var x = 0; x < _snakeGame.Settings.Width; x++)
				Print(new Coord(x, y), _empty, _gridTransform);
	}

	public void RenderSnake(IEnumerable<Coord> coords)
	{
		var array = coords as Coord[] ?? coords.ToArray();
		for (var i = 0; i < 2; i++)
			Print(array[i], i == 0 ? _snakeHead : _snake, GridValue.Snake);
	}

	public void Render(Coord coord, GridValue gridValue) 
		=> Print(coord, GetRenderDetails(gridValue), gridValue);

	public void Clear(Coord coord)
	{
		if (!_grid.ContainsKey(coord))
			return;
		
		var instance = _grid[coord];
		_grid.Remove(coord);
		Destroy(instance.gameObject);
	}

	private void Print(Coord coord, Transform prefab, Transform parent)
	{
		var position = new Vector3(-_snakeGame.Settings.Width / 2 + coord.X, _snakeGame.Settings.Height / 2 - coord.Y);
		var instance = Instantiate(prefab, position, Quaternion.identity, parent);
		instance.name = $"[{coord.X}-{coord.Y}] {parent.name}";
	}
	private void Print(Coord coord, Transform prefab, GridValue gridValue)
	{
		var position = new Vector3(-_snakeGame.Settings.Width / 2 + coord.X, _snakeGame.Settings.Height / 2 - coord.Y);
		var instance = Instantiate(prefab, position, Quaternion.identity, GetTargetTransform(gridValue));
		instance.name = $"[{coord.X}-{coord.Y}] {gridValue}";
		
		AddToGrid(coord, instance);
	}

	private Transform GetTargetTransform(GridValue gridValue)
	{
		var localTransform = transform;
		return gridValue switch
		{
			GridValue.Empty => _emptyTransform,
			GridValue.Snake => localTransform,
			GridValue.Fruit => localTransform,
			GridValue.Bomb => localTransform,
			GridValue.Border => _borderTransform,
			_ => _gridTransform
		};
	}

	private void AddToGrid(Coord coord, Transform instance)
	{
		if (!_grid.ContainsKey(coord))
			_grid.Add(coord, instance);
		else
		{
			var existing = _grid[coord];
			Destroy(existing.gameObject);
			_grid[coord] = instance;
		}
	}
	
	private Transform GetRenderDetails(GridValue gridValue)
	{
		return gridValue switch
		{
			GridValue.Border => _border,
			GridValue.Empty => _blank,
			GridValue.Snake => _snake,
			GridValue.Fruit => _fruit,
			_ => _empty
		};
	}

}
