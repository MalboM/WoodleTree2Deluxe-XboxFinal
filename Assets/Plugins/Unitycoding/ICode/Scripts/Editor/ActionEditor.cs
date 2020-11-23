using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;
using ICode;
using ICode.Actions;
using System.Collections.Generic;
using ArrayUtility=ICode.ArrayUtility;

namespace ICode.FSMEditor{
	public class ActionEditor {
		private StateInspector host;
		private State state;
		private static List<StateAction> copy;
		private static State copyState;

		private ReorderableObjectList actionList;

		public ActionEditor(State state, StateInspector host)
		{
			this.host = host;
			this.state = state;
		}
		
		public void OnEnable()
		{
			this.ResetActionList ();
		}
		
		public void OnInspectorGUI()
		{
			actionList.DoLayoutList ();
		}
		
		private void ResetActionList()
		{
			SerializedObject obj = new SerializedObject (state);
			SerializedProperty elements = obj.FindProperty ("actions");
			actionList = new ReorderableObjectList (obj,elements);

			actionList.drawHeaderCallback = delegate(Rect rect) {
				EditorGUI.LabelField(rect,"Actions");
			};

			actionList.onAddCallback = delegate(ReorderableObjectList list) {
				FsmGUIUtility.SubclassMenu<StateAction> (delegate(Type type){
					StateAction action = (StateAction)ScriptableObject.CreateInstance (type);
					action.name = type.GetCategory () + "." + type.Name;
					action.hideFlags = HideFlags.HideInHierarchy;
					state.Actions = ArrayUtility.Add<StateAction> (state.Actions, action);
					
					if (EditorUtility.IsPersistent (state)) {
						AssetDatabase.AddObjectToAsset (action, state);
						AssetDatabase.SaveAssets ();
					}
					list.index=list.count;
					EditorUtility.SetDirty(state);
				});		
			};

			actionList.drawElementCallback = delegate( int index, bool selected) {
				StateAction action = state.Actions [index];
				bool enabled = action.IsEnabled;
				if(selected){
					GUIStyle selectBackground= new GUIStyle("MeTransitionSelectHead"){
						stretchHeight=false,
						
					};
					selectBackground.overflow= new RectOffset(-1,-2,-2,2);
					GUILayout.BeginVertical(selectBackground);
				}
				action.IsOpen = GUIDrawer.ObjectTitlebar (action, action.IsOpen,ref enabled, FsmGUIUtility.ExecutableContextMenu(action,state));
				if(selected){
					GUILayout.EndVertical();
				}
				action.IsEnabled = enabled;
				if (action.IsOpen) {
					GUIDrawer.OnGUI(action);
				}
			};

			actionList.onRemoveCallback = delegate(ReorderableObjectList list) {
				StateAction action=state.Actions[list.index];
				state.Actions = ArrayUtility.Remove<StateAction> (state.Actions, action);
				FsmEditorUtility.DestroyImmediate(action);
				list.index=list.index-1;
				ErrorChecker.CheckForErrors();
				EditorUtility.SetDirty(state);
			};

			actionList.onContextClick = delegate(int index) {
				FsmGUIUtility.ExecutableContextMenu (state.Actions [index], state).ShowAsContext ();
			};

			actionList.onHeaderContextClick = delegate() {
				GenericMenu menu= new GenericMenu();
				
				if(state.Actions.Length > 0){
					menu.AddItem(new GUIContent("Copy"),false, delegate {
						copy=new List<StateAction>(state.Actions);
						copyState=state;
					});
				}else{
					menu.AddDisabledItem(new GUIContent("Copy"));
				}
				
				if(copy == null){
					copy= new List<StateAction>();
				}
				
				copy.RemoveAll(x=>x==null);
				if( copy.Count>0){
					
					menu.AddItem(new GUIContent("Paste After"),false,delegate() {
						for(int i=0;i< copy.Count;i++){
							ExecutableNode dest=FsmUtility.Copy(copy[i]);
							state.Actions=ArrayUtility.Add<StateAction>(state.Actions,(StateAction)dest);
							FsmEditorUtility.ParentChilds(state);
							EditorUtility.SetDirty(state);
						//	NodeInspector.Dirty();
							ErrorChecker.CheckForErrors();
						}
						
					});
					menu.AddItem(new GUIContent("Paste Before"),false,delegate() {
						for(int i=0;i< copy.Count;i++){
							ExecutableNode dest=FsmUtility.Copy(copy[i]);
							state.Actions=ArrayUtility.Insert<StateAction>(state.Actions,(StateAction)dest,0);
							FsmEditorUtility.ParentChilds(state);
							EditorUtility.SetDirty(state);
						//	NodeInspector.Dirty();
							ErrorChecker.CheckForErrors();
						}
					});
					if(copyState != state){
						menu.AddItem(new GUIContent("Replace"),false,delegate() {
							for(int i=0;i< state.Actions.Length;i++){
								FsmEditorUtility.DestroyImmediate(state.Actions[i]);
							}
							state.Actions= new StateAction[0];
							ResetActionList();
							
							for(int i=0;i< copy.Count;i++){
								ExecutableNode dest=FsmUtility.Copy(copy[i]);
								state.Actions=ArrayUtility.Add<StateAction>(state.Actions,(StateAction)dest);
								FsmEditorUtility.ParentChilds(state);
								EditorUtility.SetDirty(state);
							//	NodeInspector.Dirty();
								ErrorChecker.CheckForErrors();
							}
						});
					}else{
						menu.AddDisabledItem(new GUIContent("Replace"));
					}
				}else{
					menu.AddDisabledItem(new GUIContent("Paste After"));
					menu.AddDisabledItem(new GUIContent("Paste Before"));
					menu.AddDisabledItem(new GUIContent("Replace"));
				}
				menu.ShowAsContext();
			};

			this.host.Repaint();
			if(FsmEditor.instance != null)
				FsmEditor.instance.Repaint ();
		}
	}
}