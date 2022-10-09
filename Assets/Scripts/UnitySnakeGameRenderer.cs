using System;
using System.Collections.Generic;
using System.Linq;
using SnakeLib.Enums;
using SnakeLib.Interfaces;
using SnakeLib.Interfaces.UI;
using SnakeLib.Structs;
using UnityEngine;

public class UnitySnakeGameRenderer : MonoBehaviour, ISnakeGameRenderer<Color>
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
	[SerializeField] private Transform _grid;
	[SerializeField] private Transform _border;

	[Header("Other")]
	[SerializeField] private Color _backgroundColor;

	private readonly Dictionary<Coord, Transform> _transforms = new();
	private readonly Dictionary<RenderType, Transform> _children = new();
	private readonly Dictionary<RenderType, Queue<Transform>> _queue = new();

	private void Awake()
	{
		var screenRatio = Screen.height / (float)Screen.width;
		var ratio = _snakeGame.Settings.Height / (float)_snakeGame.Settings.Width;
		var height = _snakeGame.Settings.Width * ratio;
		var size = height * screenRatio;
		_camera.orthographicSize = size + 1;
		_camera.backgroundColor = _backgroundColor;

		foreach (var title in new[] { RenderType.Grid, RenderType.Border, RenderType.Empty })
		{
			var child = new GameObject(title.ToString());
			child.transform.parent = transform;
			_children.Add(title, child.transform);
		}
	}

	public void RenderBorder()
	{
		for (var y = -1; y <= _snakeGame.Settings.Height; y++)
			for (var x = -1; x <= _snakeGame.Settings.Width; x++)
				if (x < 0 || x == _snakeGame.Settings.Width || y < 0 || y == _snakeGame.Settings.Height)
					Render(new Coord(x, y), _border, RenderType.Border);
	}

	public void RenderGrid()
	{
		for (var y = 0; y < _snakeGame.Settings.Height; y++)
			for (var x = 0; x < _snakeGame.Settings.Width; x++)
				Render(new Coord(x, y), _grid, RenderType.Grid);
	}

	public void RenderSnake(IEnumerable<Coord> coords)
	{
		var array = coords as Coord[] ?? coords.ToArray();
		for (var i = 0; i < 2; i++)
			Render(array[i], i == 0 ? _snakeHead : _snake, RenderType.Snake);
	}

	public void Render(Coord coord, RenderType renderType) 
		=> Render(coord, GetPrefabFromType(renderType), renderType);

	public void Render(ITextField<Color> textFieldElement)
	{
		var renderType = textFieldElement.TextStyle.RenderType;
		var prefab = GetPrefabFromType(renderType);
		// Render(textFieldElement.Coord, GetPrefabFromType(textFieldElement.TextStyle.RenderType), textFieldElement.TextStyle.RenderType);
		var parent = GetTargetTransform(renderType);
		var instance = GetInstance(textFieldElement.Coord, prefab, parent);
		var renderer = instance.GetComponent<Renderer>();
		if (renderer)
			renderer.material.color = textFieldElement.TextStyle.Foreground;
		
		if (renderType == RenderType.Grid)
			return;
		
		AddToGrid(textFieldElement.Coord, instance, parent);
	}

	private void Render(Coord coord, Transform prefab, RenderType renderType)
	{
		// if (cellType == CellType.Empty)
		// 	return;
		
		var parent = GetTargetTransform(renderType);
		var instance = GetInstance(coord, prefab, parent);
		if (renderType == RenderType.Grid)
			return;
		
		AddToGrid(coord, instance, parent);
	}

	private Transform GetInstance(Coord coord, Transform prefab, Transform parent)
	{
		var instance = Instantiate(prefab, GetPosition(coord), Quaternion.identity, parent);
		instance.name = $"[{coord.X}-{coord.Y}] {parent.name}";
		return instance;
	}
	private Vector3 GetPosition(Coord coord) 
		=> new (-_snakeGame.Settings.Width / 2 + coord.X, _snakeGame.Settings.Height / 2 - coord.Y);

	private void AddToGrid(Coord coord, Transform prefab, Transform parent)
	{
		if (!_transforms.ContainsKey(coord))
			_transforms.Add(coord, prefab);
		else
		{
			var instance = _transforms[coord];
			Destroy(instance.gameObject);
			_transforms[coord] = prefab;
		}
	}

	private Transform GetPrefabFromType(RenderType renderType)
	{
		return renderType switch
		{
			RenderType.Border => _border,
			RenderType.Empty => _empty,
			RenderType.Grid => _grid,
			RenderType.Snake => _snake,
			RenderType.Fruit => _fruit,
			_ => _empty
		};
	}

	private Transform GetTargetTransform(RenderType renderType)
	{
		var localTransform = transform;
		return renderType switch
		{
			RenderType.Empty => _children[RenderType.Empty],
			RenderType.Grid => _children[RenderType.Grid],
			RenderType.Snake => localTransform,
			RenderType.Fruit => localTransform,
			RenderType.Bomb => localTransform,
			RenderType.Border => _children[RenderType.Border],
			_ => _children[RenderType.Empty]
		};
	}
}