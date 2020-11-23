using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ICode;

namespace ICode.FSMEditor{
	[InitializeOnLoad]
	public class HierarchyItem {
		static HierarchyItem ()
		{
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
		}

		static void HierarchyWindowItemCallback(int pID, Rect pRect)
		{
			GameObject go = EditorUtility.InstanceIDToObject(pID) as GameObject;
			if (go != null && go.GetComponent<ICodeBehaviour>() != null)
			{
				Rect rect = new Rect(pRect.x + pRect.width - 25, pRect.y-3, 25, 25);
				GUI.DrawTexture( rect,FsmEditorStyles.iCodeLogo);
				ICodeBehaviour[] behaviours=go.GetComponents<ICodeBehaviour>();
				for(int i=0;i< behaviours.Length;i++){
					ICodeBehaviour behaviour=behaviours[i];
					ErrorChecker.CheckForErrors(behaviour.stateMachine);
					if(ErrorChecker.HasErrors(behaviour.stateMachine) || behaviour.stateMachine == null){
						Rect rect1 = new Rect(pRect.x + pRect.width - 25-rect.width, pRect.y-3, 25, 25);
						GUI.DrawTexture( rect1,EditorGUIUtility.FindTexture( "d_console.erroricon" ));
					}
				}
			}

			Event ev = Event.current;
			if (ev.type == EventType.DragPerform){
				DragAndDrop.AcceptDrag();
				var selectedObjects = new List<GameObject>();
				foreach (var objectRef in DragAndDrop.objectReferences){
					if (objectRef is StateMachine){
						if(pRect.Contains(ev.mousePosition)){
							var gameObject = (GameObject)EditorUtility.InstanceIDToObject(pID);
							var componentX = gameObject.AddComponent<ICodeBehaviour>();
							componentX.stateMachine = objectRef as StateMachine;
							selectedObjects.Add(gameObject);
						}
					}
				}

				if (selectedObjects.Count == 0) {
					return;
				}
				Selection.objects = selectedObjects.ToArray();
				ev.Use();
				
			}
		}
	}
}