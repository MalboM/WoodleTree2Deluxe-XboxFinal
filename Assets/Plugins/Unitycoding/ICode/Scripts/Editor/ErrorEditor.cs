using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ICode.FSMEditor{
	public class ErrorEditor : EditorWindow {
		private bool selectedFsmErrors;
		private Vector2 scroll;
		private int index=-1;

		public static void ShowWindow()
		{
			ErrorEditor window = EditorWindow.GetWindow<ErrorEditor>("Error Checker");
			Vector2 size = new Vector2(250f, 200f);
			window.minSize = size;
			window.selectedFsmErrors = EditorPrefs.GetBool ("SelectedFSMOnly", false);
			UnityEngine.Object.DontDestroyOnLoad (window);
		}

		private void OnEnable(){
			ErrorChecker.CheckForErrors ();
		}

		private void OnGUI(){
			List<FsmError> errors = ErrorChecker.GetErrors ();
			if (selectedFsmErrors ) {
				if(FsmEditor.instance != null){
					errors=errors.FindAll(x=>x.State.Parent==FsmEditor.Active).ToList();
				}else{
					errors.Clear();
				}
			}
			//Toolbar
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			if (GUILayout.Button ("Refresh", EditorStyles.toolbarButton)) {
				ErrorChecker.CheckForErrors();
			}
			GUILayout.FlexibleSpace ();
			if(GUILayout.Button("Selected FSM Only",(selectedFsmErrors?(GUIStyle)"TE toolbarbutton" : EditorStyles.toolbarButton))) {
				selectedFsmErrors=!selectedFsmErrors;
				ErrorChecker.CheckForErrors();
				EditorPrefs.SetBool("SelectedFSMOnly",selectedFsmErrors);

			}
			GUILayout.EndHorizontal ();

			scroll = EditorGUILayout.BeginScrollView (scroll);
			for(int i=0;i<errors.Count;i++) {
				FsmError error=errors[i];
				GUIStyle style=FsmEditorStyles.elementBackground;
				if(i==index){
					style= new GUIStyle("MeTransitionSelectHead"){
						stretchHeight=false,
						
					};
					style.overflow= new RectOffset(-1,-2,-2,2);
				}
				GUILayout.BeginVertical(style);
				GUILayout.Label(error.Type.ToString());
				GUILayout.Label(error.State.Parent.Name+" : "+error.State.Name+" : "+error.ExecutableNode.name+(error.FieldInfo != null? " : "+error.FieldInfo.Name:""));		
				GUILayout.EndVertical();
				Rect elementRect =new Rect(0,i*19f*2f,Screen.width,19f*2f);
				Event ev=Event.current;
				switch (ev.rawType) {
				case EventType.MouseDown:
					if (elementRect.Contains (Event.current.mousePosition)) {
						if (Event.current.button == 0) {
							index=i;
							if(FsmEditor.instance == null){
								FsmEditor.ShowWindow ();
							}
							FsmEditor.SelectNode(error.State);
							Event.current.Use ();
						} 
					}
					break;
				}
			}
			EditorGUILayout.EndScrollView ();
		}
		
	}
}