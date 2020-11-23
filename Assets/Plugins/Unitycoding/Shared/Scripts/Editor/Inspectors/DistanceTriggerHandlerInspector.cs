using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Unitycoding{
	[CustomEditor(typeof(DistanceTriggerHandler),true)]
	public class DistanceTriggerHandlerInspector : Editor {
		private SerializedProperty script;
		private SerializedProperty triggerTag;
		private SerializedProperty triggerType;
		private SerializedProperty key;
		
		protected virtual void OnEnable(){
			script = serializedObject.FindProperty ("m_Script");
			triggerTag = serializedObject.FindProperty ("triggerTag");
			triggerType = serializedObject.FindProperty ("triggerType");
			key = serializedObject.FindProperty ("key");
		}
		
		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			EditorGUILayout.PropertyField (script);
			triggerTag.stringValue = EditorGUILayout.TagField ("Trigger Tag",triggerTag.stringValue);
			EditorGUILayout.PropertyField (triggerType);
			if (triggerType.enumValueIndex == 2) {
				EditorGUILayout.PropertyField (key);
			}
			DrawPropertiesExcluding (serializedObject,"m_Script","triggerTag","triggerType","key");
			serializedObject.ApplyModifiedProperties ();
		}
		
		protected virtual void OnSceneGUI () {
			DistanceTriggerHandler trigger = target as DistanceTriggerHandler;
			Color color = Handles.color;
			Handles.color = new Color (0f,1f,0f,0.3f);
			Handles.DrawSolidDisc(trigger.transform.position, Vector3.up, trigger.distance);
			Handles.color = new Color (0f,1f,0f,1f);
			Handles.CircleCap(0,trigger.transform.position,Quaternion.Euler(Vector3.right*90), trigger.distance);
			Handles.color = color;
		}
	}
}