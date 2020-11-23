#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("The total number of scenes.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneCount.html")]
	[System.Serializable]
	public class GetSceneCount : StateAction {
		[Shared]
		[Tooltip("Store the total number of scenes.")]
		public FsmInt sceneCount;

		public override void OnEnter ()
		{
			base.OnEnter ();
			sceneCount.Value = SceneManager.sceneCount;
			Finish ();
		}
	}
}
#endif