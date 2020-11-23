using UnityEngine;
using System.Collections;
using System;
namespace ICode.Actions.UnityApplication{
	[Obsolete("This action is obsolete. Please use SceneManager.GetActiveScene")]
	[Category(Category.Application)]
	[Tooltip("The name of the level that was last loaded.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Application-loadedLevelName.html")]
	[System.Serializable]
	public class GetLoadedLevel : StateAction {
		[NotRequired]
		[InspectorLabel("Name")]
		[Shared]
		[Tooltip("Store the level name.")]
		public FsmString _name;
		[NotRequired]
		[Shared]
		[Tooltip("Store the index of the level.")]
		public FsmInt index;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			GetLevelInfo ();
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			GetLevelInfo ();
		}
		
		private void GetLevelInfo(){
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			_name.Value = Application.loadedLevelName;
			index.Value = Application.loadedLevel;
			#else
			_name.Value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			index.Value = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
			#endif
			
		}
	}
}