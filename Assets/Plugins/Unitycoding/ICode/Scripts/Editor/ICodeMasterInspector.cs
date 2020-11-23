using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ICode.FSMEditor
{
	[InitializeOnLoad]
	[CustomEditor (typeof(ICodeMaster))]
	public class ICodeMasterInspector : Editor
	{
		private ICodeMaster master;

		static ICodeMasterInspector ()
		{
			Selection.selectionChanged += delegate {
				if (Selection.activeGameObject != null) {
					ICodeMaster master = Selection.activeGameObject.GetComponent<ICodeMaster> ();
					if (master != null) {
						if (master.components == null) {
							master.components = new List<ICodeMaster.ComponentModel> ();
						}
						ICodeBehaviour[] components = master.gameObject.GetComponents<ICodeBehaviour> ();
						for (int i = 0; i < components.Length; i++) {
							if (master.components.Find (x => x.component == components [i]) == null) {
								master.components.Add (new ICodeMaster.ComponentModel (components [i], false));
							}
							master.components [i].component.hideFlags = master.components [i].show ? HideFlags.None : HideFlags.HideInInspector;

						}
					}

				}
			};
		}

		private void OnEnable ()
		{
			master = target as ICodeMaster;
			if (master.components == null) {
				master.components = new List<ICodeMaster.ComponentModel> ();
			}
			ICodeBehaviour[] components = master.gameObject.GetComponents<ICodeBehaviour> ();
			for (int i = 0; i < components.Length; i++) {
				if (master.components.Find (x => x.component == components [i]) == null) {
					master.components.Add (new ICodeMaster.ComponentModel (components [i], false));
				}
			}

		}

		public override void OnInspectorGUI ()
		{
			master.components.RemoveAll (x => x.component == null);
			for (int i = 0; i < master.components.Count; i++) {
				ICodeMaster.ComponentModel component = master.components [i];
				if (component.component.stateMachine != null) {
					GUILayout.BeginHorizontal ();

					component.show = GUILayout.Toggle (component.show, "", Toggle);
					component.component.hideFlags = component.show ? HideFlags.None : HideFlags.HideInInspector;

					GUILayout.FlexibleSpace ();

					if (GUILayout.Button (EditorGUIUtility.FindTexture ("UnityEditor.Graphs.AnimatorControllerTool"), Button)) {
						FsmEditor.ShowWindow ();
						if (FsmEditor.instance != null) {
							FsmEditor.SelectStateMachine (component.component.stateMachine);
						}
					}
					GUILayout.EndHorizontal ();
					GUILayout.Label (master.components [i].component.stateMachine.name, Line);


				}
			}
			
			EditorUtility.SetDirty (master);
		}

		private GUIStyle button;

		private GUIStyle Button {
			get {
				if (button == null) {
					button = new GUIStyle ("label");
					button.contentOffset = new Vector2 (2, -2);
				}
				return button;
			}
		}

		private GUIStyle toggle;

		private GUIStyle Toggle {
			get {
				if (toggle == null) {
					toggle = new GUIStyle ("toggle");
					toggle.margin = new RectOffset (0, 0, 0, -3);
				}
				return toggle;
			}
		}

		private GUIStyle line;

		private GUIStyle Line {
			get {
				if (line == null) {
					line = new GUIStyle ("ShurikenLine");
					line.fontSize = 11;
					line.fontStyle = FontStyle.Bold;
					line.normal.textColor = ((GUIStyle)"label").normal.textColor;
					line.contentOffset = new Vector2 (16, -5);
				}
				return line;
			}
		}

	}
}