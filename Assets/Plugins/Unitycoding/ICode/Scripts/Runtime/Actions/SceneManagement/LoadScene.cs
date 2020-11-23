using UnityEngine;
using System.Collections;

#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine.SceneManagement;
#endif

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Loads the scene by its name or index in Build Settings.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html")]
	[System.Serializable]
	public class LoadScene : StateAction {
		[NotRequired]
		[Tooltip("Index of the scene in the Build Settings to load.")]
		public FsmInt sceneBuildIndex;
		[NotRequired]
		[Tooltip("Name of the scene to load.")]
		public FsmString sceneName;
		#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
		[Tooltip("Allows you to specify whether or not to load the scene additively.")]
		public LoadSceneMode mode;
		#endif

		public override void OnEnter ()
		{
			base.OnEnter ();

			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			if (!sceneName.IsNone) {
				Application.LoadLevel (sceneName.Value);
			} else if (!sceneBuildIndex.IsNone) {
				Application.LoadLevel (sceneBuildIndex.Value);
			} else {
				Debug.LogWarning ("Scene could not be loaded. Please provide a scene name or build index.");
			}
			#else
			if (!sceneName.IsNone) {
				SceneManager.LoadScene (sceneName.Value, mode);
			} else if (!sceneBuildIndex.IsNone) {
				SceneManager.LoadScene (sceneBuildIndex.Value, mode);
			} else {
				Debug.LogWarning ("Scene could not be loaded. Please provide a scene name or build index.");
			}
			#endif
			Finish ();
		}
	}
}