#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Number of scenes in Build Settings.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneCountInBuildSettings.html")]
	[System.Serializable]
	public class GetSceneCountInBuildSettings : StateAction {
		[Shared]
		[Tooltip("Store the number of scenes in Build Settings.")]
		public FsmInt sceneCount;

		public override void OnEnter ()
		{
			base.OnEnter ();
			sceneCount.Value = SceneManager.sceneCountInBuildSettings;
			Finish ();
		}
	}
}
#endif