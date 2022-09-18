using nGAGEOnline.Scripts.Editor;
using SnakeLib;
using SnakeLib.Enums;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
	// [CustomPropertyDrawer(typeof(SnakeSettings))]
	public class SnakeSettingsDrawer : PropertyDrawer
	{
		private const float PADDING = 4;
		
		private int _width;
		private int _height;
		private bool _wallKills;
		private bool _canWrap;
		private bool _canEatBomb;
		private bool _dynamicDifficulty;
		private Difficulty _difficulty;
		
		private SerializedProperty Width { get; set; }
		private SerializedProperty Height { get; set; }
		private SerializedProperty WallKills { get; set; }
		private SerializedProperty CanWrap { get; set; }
		private SerializedProperty CanEatBomb { get; set; }
		private SerializedProperty DynamicDifficulty { get; set; }
		private SerializedProperty Difficulty { get; set; }

		private float _baseHeight;
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Width = property.FindPropertyRelative(nameof(Width));
			Height = property.FindPropertyRelative(nameof(Height));
			WallKills = property.FindPropertyRelative(nameof(WallKills));
			CanWrap = property.FindPropertyRelative(nameof(CanWrap));
			CanEatBomb = property.FindPropertyRelative(nameof(CanEatBomb));
			DynamicDifficulty = property.FindPropertyRelative(nameof(DynamicDifficulty));
			Difficulty = property.FindPropertyRelative(nameof(Difficulty));

			var totalHeight = 0f;
			totalHeight += EditorGUI.GetPropertyHeight(Width);
			totalHeight += EditorGUI.GetPropertyHeight(Height);
			totalHeight += EditorGUI.GetPropertyHeight(WallKills);
			totalHeight += EditorGUI.GetPropertyHeight(CanWrap);
			totalHeight += EditorGUI.GetPropertyHeight(CanEatBomb);
			totalHeight += EditorGUI.GetPropertyHeight(DynamicDifficulty);
			totalHeight += EditorGUI.GetPropertyHeight(Difficulty);
			_baseHeight = base.GetPropertyHeight(property, label);
			return _baseHeight + totalHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			var rect = new Rect(position.x, position.y, position.width, _baseHeight);
			_width = EditorGUI.IntField(rect, Width.displayName, _width);
			rect.y += _baseHeight + PADDING;
			_height = EditorGUI.IntField(rect, Height.displayName, _height);
			rect.y += _baseHeight + PADDING;

			EditorGUI.MultiIntField(rect, new[] { new GUIContent(nameof(Width)), new GUIContent(nameof(Height)) }, new[] { _width, _height });
			rect.y += _baseHeight + PADDING;

			EditorGUI.EndProperty();
			// base.OnGUI(position, property, label);
		}
	}
}
