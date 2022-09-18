using System;
using UnityEditor;
using UnityEngine;

namespace nGAGEOnline.Scripts.Editor
{
	public static class EditorUI
	{
		private const int HORIZONTAL_LINE_PADDING_DEFAULT = 10;

		public static void HorizontalLine(Color color) 
			=> HorizontalLine(color, 5, HORIZONTAL_LINE_PADDING_DEFAULT);
		public static void HorizontalLine(Color color, int paddingTop, int paddingBottom)
		{
			GUILayout.Space(paddingTop + 4);
			var rect = EditorGUILayout.GetControlRect(false, 1);
			EditorGUI.DrawRect(rect, color);
			GUILayout.Space(paddingBottom);
		}

		public static bool Button(GUIContent content, bool currentValue, GUIStyle style, Color backgroundColor)
		{
			style ??= UIStyles.IconButtonStyle;

			if (currentValue)
				GUI.backgroundColor = backgroundColor;
			if (GUILayout.Button(content, style))
				currentValue = !currentValue;
			GUI.backgroundColor = Color.white;
			
			return currentValue;
		}
		
		public static bool ToggleButtons(SerializedProperty propTargetAlbedo, string label, string button1, string button2)
			=> ToggleButtons(propTargetAlbedo, label, button1, button2, new Color(.5f, 1f, .5f), new Color(1f, .5f, .5f));
		public static bool ToggleButtons(SerializedProperty propTargetAlbedo, string label, string button1, string button2, Color active, Color inactive)
		{
			var targetAlbedo = propTargetAlbedo.boolValue;
			GUILayout.Space(5);
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label(new GUIContent(label), new GUIStyle(EditorStyles.label)
				{
					fixedWidth = 150,
					alignment = TextAnchor.MiddleLeft
				});

				using (new EditorGUILayout.HorizontalScope())
				{
					GUI.backgroundColor = targetAlbedo ? active : inactive;
					if (GUILayout.Button(new GUIContent(button1)) && !targetAlbedo)
						targetAlbedo = !targetAlbedo;
					
					GUILayout.Space(5);
					
					GUI.backgroundColor = !targetAlbedo ? active : inactive;
					if (GUILayout.Button(new GUIContent(button2)) && targetAlbedo)
						targetAlbedo = !targetAlbedo;

					GUI.backgroundColor = Color.white;
				}
			}
			return targetAlbedo;
		}
		
		public static void Slider(GUIContent label, SerializedProperty property, float leftValue, float rightValue, Rect rectPosition)
		{
			label = EditorGUI.BeginProperty(rectPosition, label, property);

			EditorGUI.BeginChangeCheck();
			
			var newValue = EditorGUI.Slider(rectPosition, label, property.floatValue, leftValue, rightValue);
			if (EditorGUI.EndChangeCheck())
				property.floatValue = newValue;

			EditorGUI.EndProperty();
		}

		public static bool ToggleLeft(bool toggle, string text, GUIStyle style)
		{
			GUILayout.Space(5);
			return EditorGUILayout.ToggleLeft(text, toggle, style);
		}
		
		public static bool Toggle(bool toggle, string text, GUIStyle style) 
			=> EditorGUILayout.Toggle(text, toggle, style);

		public static void Label(string label, string text, GUIStyle style)
		{
			EditorGUILayout.LabelField(label, text, style);
		}
	}
}