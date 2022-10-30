using System;
using System.Collections.Generic;
using System.Linq;
using SnakeLib.Enums;
using SnakeLib.Interfaces;
using SnakeLib.Interfaces.UI;
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
	[SerializeField] private Transform _bomb;
	[SerializeField] private Transform _empty;
	[SerializeField] private Transform _grid;
	[SerializeField] private Transform _border;

	[Header("Other")]
	[SerializeField] private Color _backgroundColor;

	private readonly Dictionary<Coord, Transform> _transforms = new();
	private readonly Dictionary<ObjectType, Transform> _children = new();
	private readonly Dictionary<ObjectType, Queue<Transform>> _queue = new();

	private void Awake()
	{
		var screenRatio = Screen.height / (float)Screen.width;
		var ratio = _snakeGame.Settings.Height / (float)_snakeGame.Settings.Width;
		var height = _snakeGame.Settings.Width * ratio;
		var size = height * screenRatio;
		_camera.orthographicSize = size + 1;
		_camera.backgroundColor = _backgroundColor;

		foreach (var title in new[] { ObjectType.Grid, ObjectType.Border, ObjectType.Empty })
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
					Render(new Coord(x, y), _border, ObjectType.Border);
	}

	public void RenderGrid()
	{
		for (var y = 0; y < _snakeGame.Settings.Height; y++)
			for (var x = 0; x < _snakeGame.Settings.Width; x++)
				Render(new Coord(x, y), _grid, ObjectType.Grid);
	}

	public void Render(Span<Coord> coords, ObjectType objectType)
	{
		var symbol = GetPrefabFromType(objectType);
		for (var i = 0; i < coords.Length; i++)
		{
			if (objectType == ObjectType.Snake)
				symbol = i == 0 ? _snakeHead : _snake;

			Render(coords[i], symbol, objectType);
		}
	}

	public void Render<T>(ITextField textField, T textStyle) where T : ITextStyle
	{
		var ObjectType = textStyle.ObjectType;
		var prefab = GetPrefabFromType(ObjectType);
		// Render(textFieldElement.Coord, GetPrefabFromType(textFieldElement.TextStyle.ObjectType), textFieldElement.TextStyle.ObjectType);
		var parent = GetTargetTransform(ObjectType);
		var instance = GetInstance(textField.Coord, prefab, parent);
		var renderer = instance.GetComponent<Renderer>();
		if (renderer)
		{
			if (textStyle is ITextStyle<Color> style)
				renderer.material.color = style.Foreground;
			else
				renderer.material.color = TextStyleFrom(textStyle.ObjectType).Foreground;
		}
		
		if (ObjectType == ObjectType.Grid)
			return;
		
		AddToGrid(textField.Coord, instance, parent);
	}

	public void RenderSnake(IEnumerable<Coord> coords)
	{
		var array = coords as Coord[] ?? coords.ToArray();
		for (var i = 0; i < 2; i++)
			Render(array[i], i == 0 ? _snakeHead : _snake, ObjectType.Snake);
	}

	public void Render(Coord coord, ObjectType ObjectType) 
		=> Render(coord, GetPrefabFromType(ObjectType), ObjectType);

	private void Render(Coord coord, Transform prefab, ObjectType ObjectType)
	{
		// if (cellType == CellType.Empty)
		// 	return;
		
		var parent = GetTargetTransform(ObjectType);
		var instance = GetInstance(coord, prefab, parent);
		if (ObjectType == ObjectType.Grid)
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

	private Transform GetPrefabFromType(ObjectType type)
		=> RenderDetails(type).obj;
	private ITextStyle<Color> TextStyleFrom(ObjectType type)
		=> RenderDetails(type).textStyle;

	private (Transform obj, ITextStyle<Color> textStyle) RenderDetails(ObjectType objectType)
	{
		return objectType switch
		{
			ObjectType.Border => (_border, UnityTextStyle.Border),
			ObjectType.Empty => (_empty, UnityTextStyle.Grid),
			ObjectType.Snake => (_snake, UnityTextStyle.Snake),
			ObjectType.Fruit => (_fruit, UnityTextStyle.Fruit),
			ObjectType.Bomb => (_bomb, UnityTextStyle.Bomb),
			ObjectType.Grid => (_grid, UnityTextStyle.Grid),
			_ => (_empty, new UnityTextStyle(Color.white, Color.black))
		};
	}

	private Transform GetTargetTransform(ObjectType objectType)
	{
		var localTransform = transform;
		return objectType switch
		{
			ObjectType.Border => _children[ObjectType.Border],
			ObjectType.Empty => _children[ObjectType.Empty],
			ObjectType.Snake => localTransform,
			ObjectType.Fruit => localTransform,
			ObjectType.Bomb => localTransform,
			ObjectType.Grid => _children[ObjectType.Grid],
			_ => _children[ObjectType.Empty]
		};
	}
}