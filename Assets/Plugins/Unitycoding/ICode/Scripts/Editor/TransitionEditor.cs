using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;
using ICode;
using ICode.Conditions;
using ArrayUtility=ICode.ArrayUtility;

namespace ICode.FSMEditor{
	public class TransitionEditor {
		private Editor host;
		private Node node;
		private ReorderableObjectList transitionList;
		private ReorderableObjectList conditionList;
		private static Transition copy;

		public TransitionEditor(Node node, Editor host)
		{
			this.host = host;
			this.node = node;
		}

		public void OnEnable()
		{
			this.ResetTransitionList ();
			int index = node.Transitions.ToList ().FindIndex (x => x == FsmEditor.SelectedTransition);
			if (index != transitionList.index && index > -1 ) {
				transitionList.index=index;
			}
		}


		public void OnDisable(){
			//FsmEditor.SelectTransition (null);
		}

		public void OnInspectorGUI()
		{
			int index = node.Transitions.ToList ().FindIndex (x => x == FsmEditor.SelectedTransition);

			if (index != transitionList.index && index != -1 ) {
				transitionList.index=index;
			}
			transitionList.DoLayoutList ();
			GUILayout.Space (10f);

			conditionList.DoLayoutList ();
			Event ev = Event.current;
			if (ev.rawType == EventType.KeyDown && ev.keyCode == KeyCode.Delete && FsmEditor.SelectedTransition != null && EditorUtility.DisplayDialog("Delete selected transition?",FsmEditor.SelectedTransition.FromNode.Name+" -> "+FsmEditor.SelectedTransition.ToNode.Name+"\r\n\r\nYou cannot undo this action.", "Delete", "Cancel")) {
				node.Transitions = ArrayUtility.Remove (node.Transitions, FsmEditor.SelectedTransition);
				FsmEditorUtility.DestroyImmediate(FsmEditor.SelectedTransition);
				ErrorChecker.CheckForErrors();
				EditorUtility.SetDirty(node);
			}
		}

		public void ResetTransitionList()
		{
			SerializedObject obj = new SerializedObject (node);
			SerializedProperty elements = obj.FindProperty ("transitions");
			transitionList = new ReorderableObjectList (obj,elements);
			transitionList.drawHeaderCallback = delegate(Rect rect) {
				EditorGUI.LabelField(rect,"Transitions");
				EditorGUI.LabelField (new Rect (rect.width-25, rect.y, 50, 20), "Mute");	
			};

			transitionList.onSelectCallback = delegate(int index) {
				if(node.Transitions.Length>0){
					FsmEditor.SelectTransition (this.node.Transitions[index]);
					this.ResetConditionList ();
				}
			};

			transitionList.onRemoveCallback = delegate(ReorderableObjectList list) {
				Transition transition=node.Transitions[list.index];
				node.Transitions = ArrayUtility.Remove (node.Transitions, transition);
				FsmEditorUtility.DestroyImmediate(transition);
				list.index=Mathf.Clamp(list.index-1,0,list.count-1);
				ErrorChecker.CheckForErrors();
				EditorUtility.SetDirty(node);
			};
			transitionList.drawElementCallback = delegate( int index, bool selected) {
				Transition transition = node.Transitions [index];
				if(selected){
					GUIStyle selectBackground= new GUIStyle("MeTransitionSelectHead"){
						stretchHeight=false,
						
					};
					selectBackground.overflow= new RectOffset(-1,-2,-2,2);
					GUILayout.BeginVertical(selectBackground);
				}
				GUILayout.BeginHorizontal();
				for(int i=0;i<transition.Conditions.Length;i++){
					Condition condition=transition.Conditions[i];
					if(ErrorChecker.HasErrors(condition)){
						GUILayout.Label(FsmEditorStyles.errorIcon);
						break;
					}
				}
				GUILayout.Label (transition.FromNode.Name + " -> " + transition.ToNode.Name, selected?EditorStyles.whiteLabel:EditorStyles.label);
				GUILayout.FlexibleSpace();
				transition.Mute=GUILayout.Toggle(transition.Mute,GUIContent.none,GUILayout.Width(15));
				GUILayout.Space (22f);
				GUILayout.EndHorizontal();
				if(selected){
					GUILayout.EndVertical();
				}
			};

			transitionList.onReorderCallback = delegate(ReorderableObjectList list) {
				FsmEditor.SelectTransition (this.node.Transitions[list.index]);
				this.ResetConditionList ();
			};
			transitionList.onContextClick = delegate(int index) {
				GenericMenu menu = new GenericMenu ();
				menu.AddItem (new GUIContent("Remove"),false,delegate() {
					Transition transition=node.Transitions[index];
					node.Transitions = ArrayUtility.Remove (node.Transitions, transition);
					FsmEditorUtility.DestroyImmediate(transition);

					transitionList.index=Mathf.Clamp((index == transitionList.index?index-1:(index<transitionList.index?transitionList.index-1:transitionList.index)),0,node.Transitions.Length-1);
					ErrorChecker.CheckForErrors();
					EditorUtility.SetDirty(node);
				});
				menu.ShowAsContext ();
			};

			this.ResetConditionList ();
			this.host.Repaint();
			if(FsmEditor.instance != null)
				FsmEditor.instance.Repaint ();
		}



		private void ResetConditionList()
		{
			if (node.Transitions.Length == 0) {
				return;
			}
			SerializedObject obj = new SerializedObject (node.Transitions[transitionList.index]);
			SerializedProperty elements = obj.FindProperty ("conditions");
			conditionList = new ReorderableObjectList (obj,elements);

			conditionList.drawHeaderCallback = delegate(Rect rect) {
				EditorGUI.LabelField(rect,"Conditions");
			};
			conditionList.onRemoveCallback = delegate(ReorderableObjectList list) {
				Transition transition=node.Transitions[transitionList.index];
				Condition condition=transition.Conditions[list.index];
				transition.Conditions = ArrayUtility.Remove<Condition> (transition.Conditions, condition);
				FsmEditorUtility.DestroyImmediate(condition);
				list.index=list.index-1;
				ErrorChecker.CheckForErrors();
				EditorUtility.SetDirty(transition);
			};

			conditionList.onContextClick = delegate(int index) {
				FsmGUIUtility.ExecutableContextMenu(node.Transitions[transitionList.index].Conditions[index],node).ShowAsContext();
			};

			conditionList.onHeaderContextClick = delegate() {
				GenericMenu menu= new GenericMenu();
				Transition transition=node.Transitions[transitionList.index];
				Condition[] conditions=transition.Conditions;

				if(conditions.Length > 0){
					menu.AddItem(new GUIContent("Copy"),false, delegate {
						copy=transition;
					});
				}else{
					menu.AddDisabledItem(new GUIContent("Copy"));
				}
				if(copy!= null && copy.Conditions.Length>0){
					
					menu.AddItem(new GUIContent("Paste After"),false,delegate() {
						for(int i=0;i< copy.Conditions.Length;i++){
							ExecutableNode dest=FsmUtility.Copy(copy.Conditions[i]);
							transition.Conditions=ArrayUtility.Add<Condition>(transition.Conditions,(Condition)dest);
							FsmEditorUtility.ParentChilds(transition);
							EditorUtility.SetDirty(transition);
							//NodeInspector.Dirty();
						}
						ErrorChecker.CheckForErrors();
					});
					menu.AddItem(new GUIContent("Paste Before"),false,delegate() {
						for(int i=0;i< copy.Conditions.Length;i++){
							ExecutableNode dest=FsmUtility.Copy(copy.Conditions[i]);
							transition.Conditions=ArrayUtility.Insert<Condition>(transition.Conditions,(Condition)dest,0);
							FsmEditorUtility.ParentChilds(transition);
							EditorUtility.SetDirty(transition);
							//NodeInspector.Dirty();
						}
						ErrorChecker.CheckForErrors();
					});
					if(copy != transition){
						menu.AddItem(new GUIContent("Replace"),false,delegate() {
							for(int i=0;i< transition.Conditions.Length;i++){
								FsmEditorUtility.DestroyImmediate(transition.Conditions[i]);
							}
							transition.Conditions= new Condition[0];
							//ResetConditionList();
							
							for(int i=0;i< copy.Conditions.Length;i++){
								ExecutableNode dest=FsmUtility.Copy(copy.Conditions[i]);
								transition.Conditions=ArrayUtility.Add<Condition>(transition.Conditions,(Condition)dest);
								FsmEditorUtility.ParentChilds(transition);
								EditorUtility.SetDirty(transition);
								//NodeInspector.Dirty();
							}
							ErrorChecker.CheckForErrors();
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

			conditionList.onAddCallback = delegate(ReorderableObjectList list) {
				FsmGUIUtility.SubclassMenu<Condition> (delegate(Type type){
					Transition transition=node.Transitions[transitionList.index];
					Condition condition = (Condition)ScriptableObject.CreateInstance (type);
					condition.name = type.GetCategory () + "." + type.Name;
					condition.hideFlags = HideFlags.HideInHierarchy;
					transition.Conditions = ArrayUtility.Add<Condition> (transition.Conditions, condition);
					if (EditorUtility.IsPersistent (transition)) {
						AssetDatabase.AddObjectToAsset (condition, transition);
						AssetDatabase.SaveAssets ();
					}
					EditorUtility.SetDirty(transition);
				});		
			};

			conditionList.drawElementCallback = delegate( int index, bool selected) {
				Transition transition=node.Transitions[transitionList.index];
				Condition condition = transition.Conditions [index];
				bool enabled = condition.IsEnabled;
				if(selected){
					GUIStyle selectBackground= new GUIStyle("MeTransitionSelectHead"){
						stretchHeight=false,
						
					};
					selectBackground.overflow= new RectOffset(-1,-2,-2,2);
					GUILayout.BeginVertical(selectBackground);
				}
				condition.IsOpen = GUIDrawer.ObjectTitlebar (condition, condition.IsOpen,ref enabled, FsmGUIUtility.ExecutableContextMenu(condition,node));
				if(selected){
					GUILayout.EndVertical();
				}
				condition.IsEnabled = enabled;
				if (condition.IsOpen) {
					GUIDrawer.OnGUI(condition);
				}
				
			};

			this.host.Repaint();
			if(FsmEditor.instance != null)
				FsmEditor.instance.Repaint ();

		}

	}
}