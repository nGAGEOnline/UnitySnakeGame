using System.Collections.Generic;
using System.Linq;
using SnakeLib.Enums;
using SnakeLib.Interfaces;
using SnakeLib.Structs;
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
	// [SerializeField] private Color _snakeColor;
	// [SerializeField] private Color _gridColor;
	// [SerializeField] private Color _borderColor;

	private readonly Dictionary<Coord, Transform> _grid = new();
	private readonly Dictionary<string, Transform> _transforms = new();

	private void Awake()
	{
		var screenRatio = Screen.height / (float)Screen.width;
		var ratio = _snakeGame.Settings.Height / (float)_snakeGame.Settings.Width;
		var height = _snakeGame.Settings.Width * ratio;
		var size = height * screenRatio;
		_camera.orthographicSize = size + 1;
		_camera.backgroundColor = _backgroundColor;

		foreach (var title in new[] { "Grid", "Border", "Empty" })
		{
			var child = new GameObject(title);
			child.transform.parent = transform;
			_transforms.Add(title, child.transform);
		}
	}

	public void RenderBorder()
	{
		for (var y = -1; y <= _snakeGame.Settings.Height; y++)
			for (var x = -1; x <= _snakeGame.Settings.Width; x++)
				if (x < 0 || x == _snakeGame.Settings.Width || y < 0 || y == _snakeGame.Settings.Height)
					Render(new Coord(x, y), _border, _transforms["Border"]);
	}

	public void RenderGrid()
	{
		for (var y = 0; y < _snakeGame.Settings.Height; y++)
			for (var x = 0; x < _snakeGame.Settings.Width; x++)
				Render(new Coord(x, y), _empty, _transforms["Grid"]);
	}

	public void RenderSnake(IEnumerable<Coord> coords)
	{
		var array = coords as Coord[] ?? coords.ToArray();
		for (var i = 0; i < 2; i++)
			Render(array[i], i == 0 ? _snakeHead : _snake, CellType.Snake);
	}

	public void Render(Coord coord, CellType cellType) 
		=> Render(coord, GetTransformFromType(cellType), cellType);

	private Transform GetTransformFromType(CellType cellType)
	{
		return cellType switch
		{
			CellType.Border => _border,
			CellType.Empty => _blank,
			CellType.Snake => _snake,
			CellType.Fruit => _fruit,
			_ => _empty
		};
	}

	private void Render(Coord coord, Transform prefab, Transform parent) 
		=> GetInstance(coord, prefab, parent);

	private void Render(Coord coord, Transform prefab, CellType cellType)
	{
		var instance = GetInstance(coord, prefab, GetTargetTransform(cellType));
		AddToGrid(coord, instance);
	}

	private Transform GetInstance(Coord coord, Transform prefab, Transform parent)
	{
		var instance = Instantiate(prefab, GetPosition(coord), Quaternion.identity, parent);
		instance.name = $"[{coord.X}-{coord.Y}] {parent.name}";
		return instance;
	}
	private Vector3 GetPosition(Coord coord) 
		=> new (-_snakeGame.Settings.Width / 2 + coord.X, _snakeGame.Settings.Height / 2 - coord.Y);

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

	private Transform GetTargetTransform(CellType cellType)
	{
		var localTransform = transform;
		return cellType switch
		{
			CellType.Empty => _transforms["Empty"],
			CellType.Snake => localTransform,
			CellType.Fruit => localTransform,
			CellType.Bomb => localTransform,
			CellType.Border => _transforms["Border"],
			_ => _transforms["Grid"]
		};
	}
}