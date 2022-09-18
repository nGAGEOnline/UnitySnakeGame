using UnityEditor;
using UnityEngine;

namespace nGAGEOnline.Scripts.Editor
{
	public static class UIStyles
	{
		public static GUIStyle IconButtonStyle 
			=> new GUIStyle(EditorStyles.toolbarButton)
			{
				fixedWidth = 40, padding = new RectOffset(5, 5, 3, 3)
			};

		public static GUIStyle LabelStyle
			=> new GUIStyle(EditorStyles.label)
			{
				padding = new RectOffset(5, 5, 0, 0),
			};
		public static GUIStyle HelpBox => HelpBoxStyle();
		public static GUIStyle HelpBoxStyle()
		{
			var helpBox = EditorStyles.helpBox;
			return new GUIStyle(helpBox)
			{
				padding = new RectOffset(5, 5, 5, 5)
			};
		}

		public static GUIStyle TagStyle(TextAnchor alignment)
		{
			return new GUIStyle(EditorStyles.helpBox)
			{
				alignment = alignment,
				fontStyle = FontStyle.Bold,
				fontSize = 12,
				padding = new RectOffset(10, 10, 2, 4),
				margin = new RectOffset(4, 4, 0, 0)
			};
		}
		public static GUIStyle MiniButtonStyle 
			=> new GUIStyle(EditorStyles.miniButtonRight)
		{
			fontSize = 12,
			fontStyle = FontStyle.Bold,
			stretchWidth = false,
			//margin = new RectOffset(0, 0, 0, 3),
			padding = new RectOffset(15, 15, 2, 3),
		};
	}
}