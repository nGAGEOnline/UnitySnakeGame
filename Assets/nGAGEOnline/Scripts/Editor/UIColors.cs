using UnityEditor;
using UnityEngine;

namespace nGAGEOnline.Scripts.Editor
{
	public static class UIColors
	{
		public static Color ResolutionBadgeDefault => EditorGUIUtility.isProSkin 
			? new Color(0.25f, 0.25f, 0.25f, 1f) 
			: new Color(0.85f, 0.85f, 0.85f, 1f);
		public static Color ResolutionBadgeError => EditorGUIUtility.isProSkin 
			? new Color(0.475f, 0.275f, 0.275f, 1f) 
			: new Color(0.95f, 0.75f, 0.75f, 1f); 
		
		public static Color ResolutionBadgeSuccess => EditorGUIUtility.isProSkin 
			? new Color(0.275f, 0.475f, 0.275f, 1f) 
			: new Color(0.75f, 0.95f, 0.75f, 1f); 
		
		public static Color FixButtonColor => EditorGUIUtility.isProSkin 
			? new Color(1f, 0.4f, 0.4f, 1f) 
			: new Color(1f, 0.6f, 0.6f, 1f);
		
		public static Color ActiveTabColor =>  new Color(.25f, 1f, 0.5f);
		
		public static Color DetailColor => new Color(1f, 0.5f, 0.5f, 0.5f);
		public static Color NormalColor => new Color(0.5f, 0.5f, 1f, 1f);
		public static Color InfoColor =>  new Color(.5f, 0.75f, 1f);
		public static Color InfoColorAlt =>  new Color(.45f, 0.51f, .57f);
		public static Color ErrorColor => new Color(1f, .65f, .65f);
		public static Color AlertColor => new Color(1f, 0.75f, 0.25f);
		public static Color WarningColor => new Color(1f, .8f, .65f);

		public static Color DefaultColor => EditorGUIUtility.isProSkin 
			? new Color(0.25f, 0.25f, 0.25f, 1f)
			: new Color(0.85f, 0.85f, 0.85f, 1f);

		public static Color HelpBoxColor => EditorGUIUtility.isProSkin 
			? DefaultColor * 1.25f 
			: DefaultColor * 0.85f;
		
		public static Color InfoColorFaded = new Color(.5f, .75f, 1f, .3f);
		public static Color WarningColorFaded = new Color(1f, .5f, .2f, .3f);
		public static Color ErrorColorFaded = new Color(1f, .2f, .2f, .3f);

	}
}