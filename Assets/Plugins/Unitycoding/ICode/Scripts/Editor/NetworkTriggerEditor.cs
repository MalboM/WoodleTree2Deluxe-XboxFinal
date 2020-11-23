#if UNITY_5_1
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace ICode.FSMEditor{
	[CustomEditor(typeof(NetworkTrigger), true)]
	public class EventTriggerEditor : Editor
	{
		private SerializedProperty m_DelegatesProperty;
		private GUIContent m_IconToolbarMinus;
		private GUIContent m_EventIDName;
		private GUIContent[] m_EventTypes;
		private GUIContent m_AddButonContent;

		
		private void OnAddNewSelected(object index)
		{
			int num = (int)index;
			SerializedProperty mDelegatesProperty = this.m_DelegatesProperty;
			mDelegatesProperty.arraySize = mDelegatesProperty.arraySize + 1;
			SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(this.m_DelegatesProperty.arraySize - 1);
			arrayElementAtIndex.FindPropertyRelative("eventID").enumValueIndex = num;
			base.serializedObject.ApplyModifiedProperties();
		}
		
		protected virtual void OnEnable()
		{
			this.m_DelegatesProperty = base.serializedObject.FindProperty("m_Delegates");
			this.m_AddButonContent = new GUIContent("Add New Tirgger");
			this.m_EventIDName = new GUIContent(string.Empty);
			this.m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"))
			{
				tooltip = "Remove all events in this list."
			};
			string[] names = Enum.GetNames(typeof(NetworkTriggerType));
			this.m_EventTypes = new GUIContent[(int)names.Length];
			for (int i = 0; i < (int)names.Length; i++)
			{
				this.m_EventTypes[i] = new GUIContent(names[i]);
			}
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			int num = -1;
			EditorGUILayout.Space();
			Vector2 vector2 = GUIStyle.none.CalcSize(this.m_IconToolbarMinus);
			for (int i = 0; i < this.m_DelegatesProperty.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = this.m_DelegatesProperty.GetArrayElementAtIndex(i);
				SerializedProperty serializedProperty = arrayElementAtIndex.FindPropertyRelative("eventID");
				SerializedProperty serializedProperty1 = arrayElementAtIndex.FindPropertyRelative("callback");
				this.m_EventIDName.text = serializedProperty.enumDisplayNames[serializedProperty.enumValueIndex];
				EditorGUILayout.PropertyField(serializedProperty1, this.m_EventIDName, new GUILayoutOption[0]);
				Rect lastRect = GUILayoutUtility.GetLastRect();
				Rect rect = new Rect(lastRect.xMax - vector2.x - 8f, lastRect.y + 1f, vector2.x, vector2.y);
				if (GUI.Button(rect, this.m_IconToolbarMinus, GUIStyle.none))
				{
					num = i;
				}
				EditorGUILayout.Space();
			}
			if (num > -1)
			{
				this.RemoveEntry(num);
			}
			Rect rect1 = GUILayoutUtility.GetRect(this.m_AddButonContent, GUI.skin.button);
			rect1.x = rect1.x + (rect1.width - 200f) / 2f;
			rect1.width = 200f;
			if (GUI.Button(rect1, this.m_AddButonContent))
			{
				this.ShowAddTriggermenu();
			}
			base.serializedObject.ApplyModifiedProperties();
		}
		
		private void RemoveEntry(int toBeRemovedEntry)
		{
			this.m_DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
		}
		
		private void ShowAddTriggermenu()
		{
			GenericMenu genericMenu = new GenericMenu();
			for (int i = 0; i < (int)this.m_EventTypes.Length; i++)
			{
				bool flag = true;
				for (int j = 0; j < this.m_DelegatesProperty.arraySize; j++)
				{
					if (this.m_DelegatesProperty.GetArrayElementAtIndex(j).FindPropertyRelative("eventID").enumValueIndex == i)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					genericMenu.AddDisabledItem(this.m_EventTypes[i]);
				}
				else
				{
					genericMenu.AddItem(this.m_EventTypes[i], false, new GenericMenu.MenuFunction2(this.OnAddNewSelected), i);
				}
			}
			genericMenu.ShowAsContext();
			Event.current.Use();
		}
	}
}
#endif