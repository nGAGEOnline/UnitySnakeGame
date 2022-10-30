using SnakeLib.Enums;
using SnakeLib.Interfaces.UI;
using UnityEngine;

public class UnityTextStyle : ITextStyle<Color>
{
	public ObjectType ObjectType { get; }
	public Color Foreground { get; }
	public Color Background { get; }
	
	#region STATICS

	public static ITextStyle<Color> Default { get; }
	public static ITextStyle<Color> Border { get; }
	public static ITextStyle<Color> Grid { get; }
	public static ITextStyle<Color> Snake { get; }
	public static ITextStyle<Color> Fruit { get; }
	public static ITextStyle<Color> Bomb { get; }
	public static ITextStyle<Color> Score { get; }
	public static ITextStyle<Color> Title { get; }
	public static ITextStyle<Color> Moves { get; }
	public static ITextStyle<Color> Length { get; }
	public static ITextStyle<Color> Difficulty { get; }
	public static ITextStyle<Color> Coords { get; }

	#endregion

	public UnityTextStyle(Color foreground, Color background, ObjectType objectType = ObjectType.Text)
	{
		Foreground = foreground;
		Background = background;
		ObjectType = objectType;
	}
	static UnityTextStyle()
	{
		Default = new UnityTextStyle(Color.gray, Color.black);
		Border = new UnityTextStyle(new Color(0.25f, 0.25f, 0.25f), Color.gray, ObjectType.Border);
		Grid = new UnityTextStyle(new Color(0.25f, 0.25f, 0.25f), Color.black, ObjectType.Grid);
		Snake = new UnityTextStyle(Color.black, Color.green, ObjectType.Snake);
		Fruit = new UnityTextStyle(Color.black, Color.red, ObjectType.Fruit);
		Bomb = new UnityTextStyle(Color.black, Color.yellow, ObjectType.Bomb);
		Score = new UnityTextStyle(Color.cyan, Color.black);
		Title = new UnityTextStyle(Color.green, Color.black);
		Moves = new UnityTextStyle(new Color(0.25f, 0.25f, 0.25f), Color.black);
		Coords = new UnityTextStyle(Color.black, new Color(0.25f, 0.25f, 0.25f));
		Length = new UnityTextStyle(new Color(0.25f, 0.25f, 0.25f), Color.black);
		Difficulty = new UnityTextStyle(Color.black, new Color(0.25f, 0.25f, 0.25f));
	}
}
