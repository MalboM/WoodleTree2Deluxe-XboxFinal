using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Unitycoding
{
	[CustomPropertyDrawer (typeof(LabelAttribute))]
	public class LabelDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField (position, property, new GUIContent ((attribute as LabelAttribute).label));
		}

	}
}