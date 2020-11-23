using UnityEngine;
using UnityEditor;
using System.Collections;
using Unitycoding;

namespace ICode.FSMEditor{
	public static class ICodeMenu {
		[MenuItem("Tools/Unitycoding/ICode/Welcome Screen", false, 0)]
		public static void OpenWelcomeWindow()
		{
			WelcomeWindow.ShowWindow ();
		}

		[MenuItem("Tools/Unitycoding/ICode/Open Editor",false,1)]
		public static void OpenFsmEditor()
		{
			FsmEditor.ShowWindow ();
		}

		[MenuItem("Tools/Unitycoding/ICode/Review",false,3)]
		public static void OpenReviewReminder()
		{
			ReviewReminderWindow.ShowWindow ();
		}

		[MenuItem("Tools/Unitycoding/ICode/Create State Machine",false,4)]
		public static void CreateStateMachine()
		{
			FsmEditor.ShowWindow ();
			StateMachine stateMachine = AssetCreator.CreateAsset<StateMachine> (true);
			if (stateMachine == null) {
				return;			
			}
			stateMachine.color = (int)NodeColor.Blue;
			stateMachine.Name = stateMachine.name;
			
			FsmGameObject gameObject = ScriptableObject.CreateInstance<FsmGameObject> ();
			gameObject.Name="Owner";
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			gameObject.IsHidden = true;
			gameObject.IsShared = true;
			
			stateMachine.Variables = ArrayUtility.Add<FsmVariable> (stateMachine.Variables, gameObject);
			AssetDatabase.AddObjectToAsset (gameObject, stateMachine);
			AssetDatabase.SaveAssets ();
			
			
			AnyState state = FsmEditorUtility.AddNode<AnyState> (FsmEditor.Center,stateMachine);
			state.color = (int)NodeColor.Aqua;
			state.Name="Any State";
			FsmEditor.SelectStateMachine(stateMachine);
		}

		[MenuItem("Assets/Create/Unitycoding/ICode/State Machine")]
		public static void CreateStateMachineAsset()
		{
			StateMachine stateMachine = AssetCreator.CreateAsset<StateMachine> (false);
			stateMachine.color = (int)NodeColor.Blue;
			stateMachine.Name = stateMachine.name;
			
			FsmGameObject gameObject = ScriptableObject.CreateInstance<FsmGameObject> ();
			gameObject.Name="Owner";
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			gameObject.IsHidden = true;
			gameObject.IsShared = true;
			
			stateMachine.Variables = ArrayUtility.Add<FsmVariable> (stateMachine.Variables, gameObject);
			AssetDatabase.AddObjectToAsset (gameObject, stateMachine);
			AssetDatabase.SaveAssets ();
			
			
			AnyState state = FsmEditorUtility.AddNode<AnyState> (FsmEditor.Center,stateMachine);
			state.color = (int)NodeColor.Aqua;
			state.Name="Any State";
		}
		
		[MenuItem("Tools/Unitycoding/ICode/Tools/Global Variables", false)]
		public static void OpenGlobalVariablesEditor()
		{
			GlobalVariablesEditor.ShowWindow ();
		}

		[MenuItem("Tools/Unitycoding/ICode/Tools/Action Browser", false)]
		public static void OpenActionBrowser()
		{
			ActionBrowser.ShowWindow();
		}

		[MenuItem("Tools/Unitycoding/ICode/Tools/Condition Browser", false)]
		public static void OpenConditionBrowser()
		{
			ConditionBrowser.ShowWindow();
		}


		[MenuItem("Tools/Unitycoding/ICode/Tools/Fsm Tool", false)]
		public static void OpenFsmToolWindow()
		{
			FsmTool.ShowWindow ();
		}
		
		[MenuItem("Tools/Unitycoding/ICode/Tools/Error Console",false)]
		public static void OpenErrorConsole()
		{
			ErrorEditor.ShowWindow();
		}

		[MenuItem("Tools/Unitycoding/ICode/Tools/Shortcut Setup",false)]
		public static void OpenShortcutSetup()
		{
			SetupShortcutsEditor.ShowWindow();
		}

		[MenuItem("Tools/Unitycoding/ICode/Tools/MonoBehaviour Converter",false)]
		public static void OpenConverterEditor()
		{
			MonoBehaviourConverter.ShowWindow ();
		}

		[MenuItem("Tools/Unitycoding/ICode/Tools/Integrations", false)]
		public static void OpenIntegrationWindow()
		{
			IntegrationWindow.ShowWindow ();
		}

		[MenuItem ("Tools/Unitycoding/ICode/Components/ICodeBehaviour")]
		static void AddICodeBehaviour()
		{
			Selection.activeGameObject.AddComponent<ICodeBehaviour> ();
		}
		
		[MenuItem ("Tools/Unitycoding/ICode/Components/ICodeBehaviour", true)]
		static bool ValidateICodeBehaviour() {
			return Selection.activeGameObject != null;
		}

		[MenuItem ("Tools/Unitycoding/ICode/Components/OverrideVariables")]
		static void AddOverrideVariables()
		{
			Selection.activeGameObject.AddComponent<OverrideVariables> ();
		}
		
		[MenuItem ("Tools/Unitycoding/ICode/Components/OverrideVariables", true)]
		static bool ValidateOverrideVariables() {
			return Selection.activeGameObject != null;
		}

		[MenuItem ("Tools/Unitycoding/ICode/Components/ICodeTrigger")]
		static void AddICodeTrigger()
		{
			Selection.activeGameObject.AddComponent<ICodeTrigger> ();
		}
		
		[MenuItem ("Tools/Unitycoding/ICode/Components/ICodeTrigger", true)]
		static bool ValidateICodeTrigger() {
			return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<ICodeTrigger>()==null;
		}
	}
}