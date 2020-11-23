#if UNITY_5_5_OR_NEWER
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement
{
	[Category (Category.SceneManagement)]
	[Tooltip ("Unloads all GameObjects associated with the given scene.")]
	[HelpUrl ("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.UnloadScene.html")]
	[System.Serializable]
	public class UnloadScene : StateAction
	{
		[NotRequired]
		[Tooltip ("Index of the scene to unload.")]
		public FsmInt sceneIndex;
		[NotRequired]
		[Tooltip ("Name of the scene to unload.")]
		public FsmString sceneName;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (!sceneName.IsNone) {
				SceneManager.UnloadSceneAsync (sceneName.Value);
				//SceneManager.UnloadScene (sceneName.Value);
			} else if (!sceneIndex.IsNone) {
				SceneManager.UnloadSceneAsync (sceneIndex.Value);
			} 
			Finish ();
		}
	}
}
#endif