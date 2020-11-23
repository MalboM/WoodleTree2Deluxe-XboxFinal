using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ICode;
using ArrayUtility = ICode.ArrayUtility;

namespace ICode.FSMEditor
{
	public class FsmEditor : NodeEditor
	{
		public static FsmEditor instance;

		private Node[] Nodes {
			get {
				if (FsmEditor.Active == null) {
					return new Node[0];
				}
				return  FsmEditor.Active.Nodes;
			}
		}

		[SerializeField]
		private List<Node> selection = new List<Node> ();

		public static int SelectionCount {
			get {
				if (FsmEditor.instance != null) {
					return FsmEditor.instance.selection.Count;
				}
				return 0;
			}
		}

		public static List<Node> SelectedNodes {
			get {
				if (FsmEditor.instance != null) {
					return FsmEditor.instance.selection;
				}
				return null;
			}
		}

		private bool centerView;
		private Vector2 selectionStartPosition;
		private SelectionMode selectionMode;
		private Node fromNode;
		private VariableEditor variableEditor;
		private Rect variableEditorRect;
		private Rect fsmSelectionRect;
		private Rect preferencesRect;
		private Rect shortcutRect;
		private float debugProgress;
		[SerializeField]
		private MainToolbar mainToolbar;
		[SerializeField]
		private ShortcutEditor shortcutEditor;
		[SerializeField]
		private Transition selectedTransition;

		public static Transition SelectedTransition {
			get {
				if (FsmEditor.instance == null) {
					return null;
				}
				return FsmEditor.instance.selectedTransition;
			}
		}

		[SerializeField]
		private StateMachine active;

		public static StateMachine Active {
			get {
				if (FsmEditor.instance == null) {
					return null;
				}
				return FsmEditor.instance.active;
			}
		}

		[SerializeField]
		private GameObject activeGameObject;

		public static GameObject ActiveGameObject {
			get {
				if (FsmEditor.instance == null) {
					return null;
				}
				return FsmEditor.instance.activeGameObject;
			}
		}

		public static StateMachine Root {
			get {
				if (FsmEditor.Active == null) {
					return null;
				}
				return FsmEditor.Active.Root;
			}
		}

		public static FsmEditor ShowWindow ()
		{
			if (PreferencesEditor.GetBool (Preference.ShowWelcomeWindow) && Resources.FindObjectsOfTypeAll<FsmEditor> ().Length == 0) {
				WelcomeWindow.ShowWindow ();
			}
			FsmEditor window = EditorWindow.GetWindow<FsmEditor> ("FSM");
			return window;
		}

		private void OnDestroy ()
		{
			Selection.activeObject = null;
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();
			FsmEditor.instance = this;
			variableEditor = new VariableEditor ();
			if (mainToolbar == null) {
				mainToolbar = new MainToolbar ();			
			}
			if (shortcutEditor == null) {
				shortcutEditor = new ShortcutEditor ();

			}
			centerView = true;
			#if UNITY_2017_3_OR_NEWER
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			#else
			EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
			#endif
			ErrorChecker.CheckForErrors ();
		}

		#if UNITY_2017_3_OR_NEWER
		private void OnPlayModeStateChanged (PlayModeStateChange state)
		{
		if (ActiveGameObject != null) {
		ICodeBehaviour behaviour = ActiveGameObject.GetComponent<ICodeBehaviour> ();
		if (behaviour != null) {
		SelectStateMachine (behaviour.stateMachine);
		}
		}	
		}
		
#else
		private void OnPlayModeStateChanged ()
		{
			if (ActiveGameObject != null) {
				ICodeBehaviour behaviour = ActiveGameObject.GetComponent<ICodeBehaviour> ();
				if (behaviour != null) {
					SelectStateMachine (behaviour.stateMachine);
				}
			}	
		}
		#endif

		private void Update ()
		{
			if (FsmEditor.Active != null) {
				//	ErrorChecker.Update ();	
				if (EditorApplication.isPlaying) {
					debugProgress += Time.deltaTime * 30;
					if (debugProgress > 142) {
						debugProgress = 0;
					}
					FsmEditor.RepaintAll ();
				}
			}
		}



		protected override void OnGUI ()
		{
			mainToolbar.OnGUI ();
			EventType eventType = FsmEditorUtility.ReserveEvent (variableEditorRect, fsmSelectionRect, preferencesRect);
			ZoomableArea.Begin (new Rect (0f, 0f, scaledCanvasSize.width, scaledCanvasSize.height + 21), scale, IsDocked);
			Begin ();

			shortcutEditor.HandleKeyEvents ();
			if (FsmEditor.Active != null) {
				DoNodes ();
			} else {
				ZoomableArea.End ();
			}
			AcceptDragAndDrop ();
			End ();

			FsmEditorUtility.ReleaseEvent (eventType);
			PreferencesEditor.DoGUI (preferencesRect);
			shortcutEditor.DoGUI (shortcutRect);
			DoFsmSelection (fsmSelectionRect);
			variableEditor.DoGUI (variableEditorRect);
			if (centerView) {
				CenterView ();	
				centerView = false;
			}
			if (FsmEditor.Active != null) {
				GUI.Label (new Rect (5, 20, 300, 200), FsmEditor.Active.comment, FsmEditorStyles.instructionLabel);
			} else {
				GUI.Label (new Rect (5, 20, 300, 200), "Right click to create a state machine.", FsmEditorStyles.instructionLabel);
			}
			Event ev = Event.current;
			if (SelectedNodes.Count == 1 && ev.rawType == EventType.KeyDown && ev.keyCode == KeyCode.Delete && FsmEditor.SelectedTransition != null && EditorUtility.DisplayDialog ("Delete selected transition?", FsmEditor.SelectedTransition.FromNode.Name + " -> " + FsmEditor.SelectedTransition.ToNode.Name + "\r\n\r\nYou cannot undo this action.", "Delete", "Cancel")) {
				Node node = SelectedTransition.FromNode;
				node.Transitions = ArrayUtility.Remove (node.Transitions, FsmEditor.SelectedTransition);
				FsmEditorUtility.DestroyImmediate (FsmEditor.SelectedTransition);
				ErrorChecker.CheckForErrors ();
				EditorUtility.SetDirty (node);
			}
		}

		private void OnSelectionChange ()
		{
			FsmEditor.SelectGameObject (Selection.activeGameObject);
		}

		private void DoFsmSelection (Rect rect)
		{
			if (FsmEditor.Root == null) {
				return;			
			}
			GUILayout.BeginArea (rect, EditorStyles.toolbar);
			GUILayout.BeginHorizontal ();

			StateMachine parent = FsmEditor.Active;
			List<StateMachine> breadcrumbs = new List<StateMachine> ();

			while (parent != null) {
				breadcrumbs.Add (parent);
				parent = parent.Parent;
			}
			breadcrumbs.Reverse ();
			for (int i = 0; i < breadcrumbs.Count; i++) {
				GUIStyle style = i == 0 ? FsmEditorStyles.breadcrumbLeft : FsmEditorStyles.breadcrumbMiddle;
				GUIContent content = new GUIContent (breadcrumbs [i].Name);
				float width = style.CalcSize (content).x;
				width = Mathf.Clamp (width, 80f, width);
				if (GUILayout.Button (content, style, GUILayout.Width (width))) {
					FsmEditor.SelectStateMachine (breadcrumbs [i]);
				}
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();
		}

		protected override Rect GetCanvasSize ()
		{
			float variableHeight = Mathf.Clamp (variableEditor.GetEditorHeight (), 0, canvasSize.height * 0.5f);
			variableEditorRect = new Rect (-EditorStyles.inspectorDefaultMargins.padding.left, canvasSize.height - variableHeight + canvasSize.y, 250, variableHeight);
			fsmSelectionRect = new Rect (variableEditorRect.width - EditorStyles.inspectorDefaultMargins.padding.left - 4f, canvasSize.height, canvasSize.width, 22f);
			preferencesRect = PreferencesEditor.GetBool (Preference.ShowPreference) ? new Rect (canvasSize.width - 202f, 18f, 200f, PreferencesEditor.GetHeight ()) : new Rect ();
			shortcutRect = new Rect (canvasSize.width - 250, (FsmEditor.Active == null ? 17f : 0f), 250, canvasSize.height);
			return new Rect (0, 17f, position.width, position.height - 17f);
		}

		private void AcceptDragAndDrop ()
		{
			EventType eventType = Event.current.type;
			bool isAccepted = false;
			if ((eventType == EventType.DragUpdated || eventType == EventType.DragPerform)) {
				DragAndDrop.visualMode = DragAndDropVisualMode.Link;
				
				if (eventType == EventType.DragPerform) {
					DragAndDrop.AcceptDrag ();
					isAccepted = true;
				}
				Event.current.Use ();
			}
			
			if (isAccepted && DragAndDrop.objectReferences [0].GetType () == typeof(StateMachine)) {
				Pasteboard.Copy (new List<Node> (){ DragAndDrop.objectReferences [0] as StateMachine });
				Pasteboard.Paste (Event.current.mousePosition + scrollPosition, FsmEditor.Active);
			}
		}

		private void DoNodeEvents ()
		{
			if (currentEvent.button != 0) {
				return;			
			} 
			SelectNodes ();
			DragNodes ();
		}

		private void SelectNodes ()
		{
			int controlID = GUIUtility.GetControlID (FocusType.Passive);
			switch (currentEvent.rawType) {
			case EventType.MouseDown:
				GUIUtility.hotControl = controlID;
				selectionStartPosition = mousePosition;
				Node node = MouseOverNode ();
				Transition transition = MouseOverTransition ();
				if (transition != null && node == null) {

					this.selection.Clear ();
					this.selection.Add (transition.FromNode);
					UpdateUnitySelection ();
					SelectTransition (transition);
					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
					Event.current.Use ();
					return;
				}
				if (node != null) {
					if (!SelectedNodes.Contains (node)) {
						SelectTransition (null);
					}
					if (fromNode != null) {
						AddTransition (fromNode, node);
						fromNode = null;
						GUIUtility.hotControl = 0;
						GUIUtility.keyboardControl = 0;
						return;
					}
					if (node.GetType () == typeof(StateMachine) && currentEvent.clickCount == 2) {
						SelectStateMachine ((StateMachine)node);	
					} else {
						if (EditorGUI.actionKey || currentEvent.shift) {
							if (!this.selection.Contains (node)) {
								this.selection.Add (node);
							} else {
								this.selection.Remove (node);
							}
						} else if (!this.selection.Contains (node)) {
							this.selection.Clear ();
							this.selection.Add (node);
						}
					}

					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
					UpdateUnitySelection ();
					return;
				}
				fromNode = null;
				selectionMode = SelectionMode.Pick;
				if (!EditorGUI.actionKey && !currentEvent.shift) {
					this.selection.Clear ();
					SelectTransition (null);
					UpdateUnitySelection ();

				}
				currentEvent.Use ();
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID) {
					selectionMode = SelectionMode.None;
					GUIUtility.hotControl = 0;
					currentEvent.Use ();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID && !EditorGUI.actionKey && !currentEvent.shift && (selectionMode == SelectionMode.Pick || selectionMode == SelectionMode.Rect)) {
					selectionMode = SelectionMode.Rect;	
					SelectNodesInRect (FromToRect (selectionStartPosition, mousePosition));
					currentEvent.Use ();
				}
				break;
			case EventType.Repaint:
				if (GUIUtility.hotControl == controlID && selectionMode == SelectionMode.Rect) {
					FsmEditorStyles.selectionRect.Draw (FromToRect (selectionStartPosition, mousePosition), false, false, false, false);		
				}
				break;
			}
		}

		private void DragNodes ()
		{
			int controlID = GUIUtility.GetControlID (FocusType.Passive);

			switch (currentEvent.rawType) {
			case EventType.MouseDown:
				GUIUtility.hotControl = controlID;
				currentEvent.Use ();
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID) {
					GUIUtility.hotControl = 0;
					currentEvent.Use ();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID) {
					for (int i = 0; i < selection.Count; i++) {
						Node node = selection [i];
						node.position.position += currentEvent.delta;
					}
					currentEvent.Use ();
				}
				break;
			case EventType.Repaint:
				if (GUIUtility.hotControl == controlID) {
					AutoPanNodes (1.5f);
				}
				break;
			}
		}

		private void AutoPanNodes (float speed)
		{
			Vector2 delta = Vector2.zero;
			if (mousePosition.x > scaledCanvasSize.width + scrollPosition.x - 50f) {
				delta.x += speed;
			} 
			
			if ((mousePosition.x < scrollPosition.x + 50f) && scrollPosition.x > 0f) {
				delta.x -= speed;
			} 
			
			if (mousePosition.y > scaledCanvasSize.height + scrollPosition.y - 50f) {
				delta.y += speed;
			} 
			
			if ((mousePosition.y < scrollPosition.y + 50f) && scrollPosition.y > 0f) {
				delta.y -= speed;
			}

			if (delta != Vector2.zero) {

				for (int i = 0; i < selection.Count; i++) {
					Node node = selection [i];
					node.position.position += delta;
				}
				UpdateScrollPosition (scrollPosition + delta);
				Repaint ();
			}
		}

		private void DoNodes ()
		{
			DoTransitions ();
			for (int i = 0; i < Nodes.Length; i++) {
				Node node = Nodes [i];
				if (!selection.Contains (node)) {
					DoNode (node);			
				}
			}

			for (int i = 0; i < selection.Count; i++) {
				Node node = selection [i];
				DoNode (node);			
			}

			DoNodeEvents ();
			ZoomableArea.End ();
			NodeContextMenu ();
		}

		private void DoNode (Node node)
		{
			GUIStyle style = FsmEditorStyles.GetNodeStyle (node.color, selection.Contains (node), node.GetType () == typeof(StateMachine));
			string state = string.Empty;
			if (FsmEditor.Active.Owner != null && FsmEditor.Active.Owner.ActiveNode && (node == FsmEditor.Active.Owner.ActiveNode || node == FsmEditor.Active.Owner.AnyState || node == FsmEditor.Active.Owner.ActiveNode.Parent) && EditorApplication.isPlaying) {
				if (!FsmEditor.Active.Owner.IsPaused && !FsmEditor.Active.Owner.IsDisabled) {
					state = "[Active]";
				} else if (FsmEditor.Active.Owner.IsPaused) {
					state = "[Paused]";
				} else if (FsmEditor.Active.Owner.IsDisabled) {
					state = "[Disabled]";
				}

			}

			GUI.Box (node.position, node.Name + state, style);

			if (ErrorChecker.HasErrors (node) && Event.current.type != EventType.Layout) {
				Rect rect = node.position;
				if (node is StateMachine) {
					rect.x += 10;
					rect.y += 6;
				}
				GUI.Label (rect, "", "CN EntryError");
			}

			if (node is State && (node as State).IsSequence) {
				Rect rect = node.position;
				rect.x += 25;
				rect.y += 3;
				GUI.Label (rect, EditorGUIUtility.FindTexture ("d_PlayButtonProfile"));			
			} 

			if (PreferencesEditor.GetBool (Preference.ShowStateDescription)) {
				GUILayout.BeginArea (new Rect (node.position.x, node.position.y + node.position.height, node.position.width, 500));
				GUILayout.Label (node.comment, FsmEditorStyles.wrappedLabel);
				GUILayout.EndArea ();
			}

			if (DisplayProgress (node)) {
				Rect rect = new Rect (node.position.x + 5, node.position.y + 20, debugProgress, 5);

				if (node == FsmEditor.Active.Owner.ActiveNode.Parent) {
					rect.y += 5;
					rect.x += 15;
					rect.width *= 0.8f;
				}
				GUI.Box (rect, "", "MeLivePlayBar");
			}	

		}

		private bool DisplayProgress (Node node)
		{
			return  (FsmEditor.Active.Owner != null && FsmEditor.Active.Owner.ActiveNode && !FsmEditor.Active.Owner.IsPaused && !FsmEditor.Active.Owner.IsDisabled && (node == FsmEditor.Active.Owner.ActiveNode || node == FsmEditor.Active.Owner.AnyState || node == FsmEditor.Active.Owner.ActiveNode.Parent) && EditorApplication.isPlaying);
		}

		private void DoTransitions ()
		{
			if (fromNode != null) {
				DrawConnection (fromNode.position.center, mousePosition, Color.green, 1, false);
				Repaint ();
			}
			for (int i = 0; i < Nodes.Length; i++) {
				Node node = Nodes [i];
				DoTransition (node);
			}
		}

		
		private void DoTransition (Node node)
		{
			var groups = node.Transitions.GroupBy (c => c.ToNode).ToList ();
			foreach (var group in groups) {   
				Node toNode = group.First ().ToNode;
				Node fromNode = group.First ().FromNode;
				int arrowCount = group.Count () > 1 ? 3 : 1;
				bool offset = toNode.Transitions.Any (x => x.ToNode == fromNode);
				Color color = group.Any (x => x == FsmEditor.SelectedTransition) ? Color.cyan : Color.white;
				DrawConnection (fromNode.position.center, toNode.position.center, color, arrowCount, offset); 

			}
		}

		private void AddTransition (Node fromNode, Node toNode)
		{
			if (fromNode == null || toNode == null || toNode.GetType () == typeof(AnyState)) {
				return;
			}
			Transition transition = ScriptableObject.CreateInstance<Transition> ();
			transition.hideFlags = HideFlags.HideInHierarchy;
			transition.Init (toNode, fromNode);
			if (EditorUtility.IsPersistent (fromNode)) {
				AssetDatabase.AddObjectToAsset (transition, fromNode);
				AssetDatabase.SaveAssets ();
			}
			fromNode.Transitions = ArrayUtility.Add<Transition> (fromNode.Transitions, transition);
			NodeInspector.Dirty ();	
		}

		protected override void CanvasContextMenu ()
		{
			if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1) {
				return;
			}	
			GenericMenu canvasMenu = new GenericMenu ();

			if (FsmEditor.Active == null) {
				canvasMenu.AddItem (new GUIContent ("Create State Machine"), false, delegate() {
					ICodeMenu.CreateStateMachine ();
				});
				canvasMenu.ShowAsContext ();
				return;
			}

			canvasMenu.AddItem (FsmContent.createState, false, delegate() {
				State state = FsmEditorUtility.AddNode<State> (mousePosition, FsmEditor.Active);
				state.IsStartNode = FsmEditor.Active.GetStartNode () == null;
				FsmEditorUtility.UpdateNodeColor (state);
			});
			canvasMenu.AddItem (FsmContent.createSubFsm, false, delegate() {
				StateMachine stateMachine = FsmEditorUtility.AddNode<StateMachine> (mousePosition, FsmEditor.Active);
				stateMachine.IsStartNode = FsmEditor.Active.GetStartNode () == null;
				FsmEditorUtility.UpdateNodeColor (stateMachine);
			});

			canvasMenu.AddItem (FsmContent.copy, false, delegate() {
				Pasteboard.Copy (new List<Node> (){ FsmEditor.Active });
			});

			if (Pasteboard.CanPaste ()) {
				canvasMenu.AddItem (FsmContent.paste, false, delegate() {
					Pasteboard.Paste (mousePosition, FsmEditor.Active);
				});
			}
			canvasMenu.AddSeparator ("");
			if (Selection.activeGameObject != null) {
				canvasMenu.AddItem (FsmContent.addToSelection, false, delegate() {
					foreach (GameObject go in Selection.gameObjects) {
						ICodeBehaviour behaviour = go.AddComponent<ICodeBehaviour> ();
						behaviour.stateMachine = FsmEditor.Active.Root;
						EditorUtility.SetDirty (behaviour);

					}
					SelectGameObject (Selection.activeGameObject);
				});
				canvasMenu.AddItem (FsmContent.bindToGameObject, false, delegate() {
					foreach (GameObject go in Selection.gameObjects) {
						ICodeBehaviour behaviour = go.AddComponent<ICodeBehaviour> ();
						behaviour.stateMachine = (StateMachine)FsmUtility.Copy (FsmEditor.Active.Root);//FsmEditor.Active.Root;
						EditorUtility.SetDirty (behaviour);
						
					}
					SelectGameObject (Selection.activeGameObject);
				});
			} else {
				canvasMenu.AddDisabledItem (FsmContent.addToSelection);	
				canvasMenu.AddDisabledItem (FsmContent.bindToGameObject);
			}

			if (FsmEditor.Active.Root != null && !EditorUtility.IsPersistent (FsmEditor.Active.Root)) {
				canvasMenu.AddItem (FsmContent.saveAsAsset, false, delegate() {
					string mPath = EditorUtility.SaveFilePanelInProject (
						               "Save StateMachine as Asset",
						               "New StateMachine.asset",
						               "asset", "");
					if (mPath != null) {
						StateMachine stateMachine = (StateMachine)FsmUtility.Copy (FsmEditor.Active.Root);
						AssetDatabase.CreateAsset (stateMachine, mPath);
						AssetDatabase.SaveAssets ();
						FsmEditorUtility.ParentChilds (stateMachine);
					}
				});
			} else {
				canvasMenu.AddDisabledItem (FsmContent.saveAsAsset);			
			}
			canvasMenu.ShowAsContext ();
		}

		private void NodeContextMenu ()
		{
			if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1) {
				return;
			}	

			Node node = MouseOverNode ();
			if (node == null) {
				return;			
			}
			GenericMenu nodeMenu = new GenericMenu ();
			if (Application.isPlaying && this.active != null && this.active.Owner != null && node != this.active.Owner.ActiveNode) {
				nodeMenu.AddItem (new GUIContent ("Execute"), false, delegate {
					this.active.Owner.SetNode (node);
				});
			}

			nodeMenu.AddItem (FsmContent.makeTransition, false, delegate() {
				fromNode = node;
			});


			if (!node.IsStartNode && !(node is AnyState)) {
				nodeMenu.AddItem (FsmContent.setAsDefault, false, delegate() {
					FsmEditorUtility.SetDefaultNode (node, FsmEditor.Active);
				});
			} else {
				nodeMenu.AddDisabledItem (FsmContent.setAsDefault);
			}

			if (node.GetType () == typeof(State)) {
				State state = node as State;
				nodeMenu.AddItem (FsmContent.sequence, state.IsSequence, delegate() {
					state.IsSequence = !state.IsSequence;
				});
			}

			if (node.GetType () != typeof(AnyState)) {
				nodeMenu.AddItem (FsmContent.moveToSubStateMachine, false, delegate() {
					StateMachine stateMachine = FsmEditorUtility.AddNode<StateMachine> (mousePosition, FsmEditor.Active);
					Pasteboard.Copy (selection);
					Pasteboard.Paste (mousePosition, stateMachine);
					foreach (Node mNode in selection) {
						if (!(mNode is AnyState)) {
							FsmEditorUtility.DeleteNode (mNode);
						}
					}
					selection.Clear ();
					UpdateUnitySelection ();
					EditorUtility.SetDirty (FsmEditor.Active);
				});

				if (FsmEditor.Active.Parent != null) {
					nodeMenu.AddItem (FsmContent.moveToParentStateMachine, false, delegate() {
						Pasteboard.Copy (selection);
						Pasteboard.Paste (mousePosition, FsmEditor.Active.Parent);
						foreach (Node mNode in selection) {
							if (!(mNode is AnyState)) {
								FsmEditorUtility.DeleteNode (mNode);
							}
						}
						selection.Clear ();
						UpdateUnitySelection ();
						EditorUtility.SetDirty (FsmEditor.Active);
					});	
				} else {
					nodeMenu.AddDisabledItem (FsmContent.moveToParentStateMachine);
				}

				nodeMenu.AddItem (FsmContent.copy, false, delegate() {
					Pasteboard.Copy (selection);
				});
				
				nodeMenu.AddItem (FsmContent.delete, false, delegate() {
					if (selection.Contains (node)) {
						foreach (Node mNode in selection) {
							if (!(mNode is AnyState)) {
								FsmEditorUtility.DeleteNode (mNode);
							}
						}
						selection.Clear ();
						UpdateUnitySelection ();
					} else {
						FsmEditorUtility.DeleteNode (node);
					}
					EditorUtility.SetDirty (FsmEditor.Active);
				});
			} else {
				nodeMenu.AddDisabledItem (FsmContent.copy);
				nodeMenu.AddDisabledItem (FsmContent.delete);
			}
			nodeMenu.ShowAsContext ();
			Event.current.Use ();
		}

		private Node MouseOverNode ()
		{
			for (int i = 0; i < Nodes.Length; i++) {
				Node node = Nodes [i];
				if (node.position.Contains (mousePosition)) {
					return node;
				}			
			}		
			return null;
		}

		private Transition MouseOverTransition ()
		{
			for (int i = 0; i < Nodes.Length; i++) {
				Node node = Nodes [i];
				for (int j = 0; j < node.Transitions.Length; j++) {
					Transition transition = node.Transitions [j];
					bool offset = transition.ToNode.Transitions.Any (x => x.ToNode == transition.FromNode);
					Vector3 start = transition.FromNode.position.center;
					Vector3 end = transition.ToNode.position.center;
					Vector3 cross = Vector3.Cross ((start - end).normalized, Vector3.forward);
					if (offset) {
						start = start + cross * 6;
						end = end + cross * 6;
					}
					if (HandleUtility.DistanceToLine (start, end) < 3f) {
						return transition;
					}
				}
			}		
			return null;
		}

		private void SelectNodesInRect (Rect r)
		{
			for (int i = 0; i < Nodes.Length; i++) {
				Node node = Nodes [i];
				Rect rect = node.position;
				if (rect.xMax < r.x || rect.x > r.xMax || rect.yMax < r.y || rect.y > r.yMax) {
					selection.Remove (node);
					continue;
				}
				if (!selection.Contains (node)) {
					selection.Add (node);
				}
			}
			UpdateUnitySelection ();
		}

		private Rect FromToRect (Vector2 start, Vector2 end)
		{
			Rect rect = new Rect (start.x, start.y, end.x - start.x, end.y - start.y);
			if (rect.width < 0f) {
				rect.x = rect.x + rect.width;
				rect.width = -rect.width;
			}
			if (rect.height < 0f) {
				rect.y = rect.y + rect.height;
				rect.height = -rect.height;
			}
			return rect;
		}

		private void UpdateUnitySelection ()
		{
			Selection.objects = selection.ToArray ();		
		}

		public void ToggleSelection ()
		{
			if (FsmEditor.Active == null) {
				return;			
			}
			if (selection.Count == FsmEditor.Active.Nodes.Length) {
				selection.Clear ();
			} else {
				selection.Clear ();
				selection.AddRange (FsmEditor.Active.Nodes);
			}
			UpdateUnitySelection ();
		}

		public static void SelectStateMachine (StateMachine stateMachine)
		{
			if (FsmEditor.instance == null || FsmEditor.Active == stateMachine) {
				FsmEditor.instance.CenterView ();	
				return;			
			}
			FsmEditor.instance.active = stateMachine;
			FsmEditor.instance.selection.Clear ();
			FsmEditor.instance.UpdateUnitySelection ();
			FsmEditor.instance.CenterView ();	
			ErrorChecker.CheckForErrors ();
		}

		public static void SelectGameObject (GameObject gameObject)
		{
			if (FsmEditor.instance == null) { //|| FsmEditor.ActiveGameObject == gameObject) {
				return;			
			}

			if (!PreferencesEditor.GetBool (Preference.LockSelection) && gameObject != null) {
				ICodeBehaviour behaviour = gameObject.GetComponent<ICodeBehaviour> ();
				if (behaviour != null && behaviour.stateMachine != null) {
					FsmEditor.instance.activeGameObject = behaviour.gameObject;
					FsmEditor.SelectStateMachine (behaviour.stateMachine);
				}	
			}
		}

		public static void SelectTransition (Transition transition)
		{
			if (FsmEditor.instance == null || FsmEditor.SelectedTransition == transition) {
				return;			
			}
			FsmEditor.instance.selectedTransition = transition;
			FsmEditor.instance.Repaint ();
		}

		public static void SelectNode (Node node)
		{
			if (FsmEditor.instance == null) {
				return;			
			}
			if (FsmEditor.instance.active != node.Parent) {
				FsmEditor.SelectStateMachine (node.Parent);
			}
			FsmEditor.instance.selection.Clear ();
			FsmEditor.instance.selection.Add (node);
			FsmEditor.instance.UpdateUnitySelection ();
			FsmEditor.instance.Repaint ();
		}


		public void CenterView ()
		{
			Vector2 center = Vector2.zero;
			if (Nodes.Length > 0) {
				for (int i = 0; i < Nodes.Length; i++) {
					Node node = Nodes [i];
					center += new Vector2 (node.position.center.x - scaledCanvasSize.width * 0.5f, node.position.center.y - scaledCanvasSize.height * 0.5f);
				}	
				center /= Nodes.Length;
			} else {
				center = NodeEditor.Center;
			}
			UpdateScrollPosition (center);
			Repaint ();
		}

		public static void RepaintAll ()
		{
			if (FsmEditor.instance != null) {
				FsmEditor.instance.Repaint ();			
			}		
		}

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
		[DrawGizmo(GizmoType.SelectedOrChild | GizmoType.NotSelected)]
		
#else
		[DrawGizmo (GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
		#endif
		static void DrawGameObjectName (Transform transform, GizmoType gizmoType)
		{   
			ICodeBehaviour behaviour = transform.GetComponent<ICodeBehaviour> ();
			if (behaviour == null) {
				return;
			}
			if (behaviour.showSceneIcon) {
				Handles.Label (transform.position, FsmEditorStyles.iCodeLogo);
			}
			if (behaviour.showStateGizmos && behaviour.stateMachine != null) { 
				Node activeNode = behaviour.ActiveNode;
				if (activeNode != null) {
					Handles.Label (transform.position, activeNode.Name, FsmEditorStyles.stateLabelGizmo);
				}
			}
		}

		public bool IsDocked {
			get {
				BindingFlags fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
				MethodInfo isDockedMethod = typeof(EditorWindow).GetProperty ("docked", fullBinding).GetGetMethod (true);
				return (bool)isDockedMethod.Invoke (this, null);
			}
		}

		public enum SelectionMode
		{
			None,
			Pick,
			Rect,
		}
	}
}