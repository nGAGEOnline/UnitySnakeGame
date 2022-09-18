using SnakeLib.Structs;
using UnityEngine;

namespace Helpers
{
	public static class Extensions
	{
		public static Vector2 ToVector2(this Coord coord) => new Vector2(coord.X, coord.Y);
		public static Vector3 ToVector3(this Coord coord) => new Vector3(coord.X, coord.Y, 0);
	}
}
