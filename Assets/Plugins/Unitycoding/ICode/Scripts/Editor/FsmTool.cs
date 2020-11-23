using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Unitycoding;

namespace ICode.FSMEditor{
	public class FsmTool : EditorWindow {
		public static FsmTool ShowWindow()
		{
			FsmTool window = EditorWindow.GetWindow<FsmTool>("FsmTool");
			return window;
		}

		[SerializeField]
		private StateMachine[] fsms;
		[SerializeField]
		private GameObject[] targets;

		[SerializeField]
		private Vector2 scroll;
		[SerializeField]
		private int index = -1;
		[SerializeField]
		private int dragIndex = -1;
		[SerializeField]
		private FsmTool.Loaction location;

		private void OnEnable(){
			location = (FsmTool.Loaction)EditorPrefs.GetInt ("FsmToolLocation",0);
			FindFsms ();

		}

		private void OnDisable(){
			EditorPrefs.SetInt ("FsmToolLocation", (int)location);
		}
		
		private void OnGUI(){
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			if (GUILayout.Button ("Refresh", EditorStyles.toolbarButton)) {
				FindFsms();
			}
			GUILayout.FlexibleSpace ();
			FsmTool.Loaction mloc = (FsmTool.Loaction)EditorGUILayout.EnumPopup (location, EditorStyles.toolbarPopup,GUILayout.Width(100f));
			if (location != mloc) {
				location=mloc;
				FindFsms();
			}
			GUILayout.EndHorizontal ();
			scroll = EditorGUILayout.BeginScrollView (scroll);
			for(int i=0;i<fsms.Length;i++) {
				StateMachine fsm=fsms[i];
				GUIStyle style=FsmEditorStyles.elementBackground;
				if(i==index){
					style= new GUIStyle("MeTransitionSelectHead"){
						stretchHeight=false,
						
					};
					style.overflow= new RectOffset(-1,-2,-2,2);
				}
				GUILayout.BeginVertical(style);
				GUILayout.Label(location== Loaction.Project? AssetDatabase.GetAssetPath(fsm):targets[i]+" : "+fsm.Name);
					
				Rect elementRect =GUILayoutUtility.GetLastRect();
				Event ev=Event.current;
				switch (ev.rawType) {
				case EventType.MouseDown:
					if (elementRect.Contains (Event.current.mousePosition)) {
						if (Event.current.button == 0) {
							DragAndDrop.objectReferences=new Object[1]{fsm};
							dragIndex=i;
							Event.current.Use ();
						} 
					}
					break;
				case EventType.MouseUp:
					if (elementRect.Contains (Event.current.mousePosition)) {
						if (Event.current.button == 0) {
							index=i;
							if(FsmEditor.instance == null){
								FsmEditor.ShowWindow ();
							}
							if(location == Loaction.Scene){
								FsmEditor.SelectGameObject(targets[i]);
							}
							FsmEditor.SelectStateMachine(fsm);
							Event.current.Use ();
						} 
					}
					dragIndex=-1;
					break;
				case EventType.MouseDrag:
					if(dragIndex != -1){
						DragAndDrop.StartDrag("Drag");
						dragIndex=-1;
						Event.current.Use();
					}
					break;
				}
				GUILayout.EndVertical();

			}
			EditorGUILayout.EndScrollView ();
		}

		private void FindFsms(){
			if (location == Loaction.Project) {
				fsms = UnityEditorUtility.GetAssetsOfType<StateMachine> (".asset");
			} else {
				ICodeBehaviour[] behaviours=UnityEditorUtility.FindInScene<ICodeBehaviour>().ToArray();
				fsms=behaviours.Select(x=>x.stateMachine).ToArray();
				targets=behaviours.Select(x=>x.gameObject).ToArray();
			}
		}

		public enum Loaction{
			Project,
			Scene
		}
	}
}