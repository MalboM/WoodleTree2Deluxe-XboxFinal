using UnityEngine;
using System.Collections;

#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine.SceneManagement;
#endif


namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Loads the scene asynchronously in the background.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html")]
	[System.Serializable]
	public class LoadSceneAsync : StateAction {
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

		[NotRequired]
		[Tooltip("Send event when loading is done. Check with OnCustomEvent")]
		public FsmString loadedEvent;
		[NotRequired]
		[Shared]
		[Tooltip("Store the progress of loading.")]
		public FsmFloat progress;

		private AsyncOperation asyncOperation;

		public override void OnEnter ()
		{
			base.OnEnter ();
			this.Root.Owner.StartCoroutine (LoadScene ());
		}

		private IEnumerator LoadScene(){
			if (Time.time < 2f) {
				yield return new WaitForSeconds (1f);
			}

			if (!sceneName.IsNone) {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				asyncOperation = Application.LoadLevelAsync (sceneName.Value);
				#else
				asyncOperation = SceneManager.LoadSceneAsync (sceneName.Value, mode);
				#endif
			} else if (!sceneBuildIndex.IsNone) {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				asyncOperation = Application.LoadLevelAsync (sceneBuildIndex.Value);
				#else
				asyncOperation = SceneManager.LoadSceneAsync (sceneBuildIndex.Value, mode);
				#endif
			} else {
				Debug.LogWarning ("Scene could not be loaded. Please provide a scene name or build index.");
				Finish ();
				yield break;
			}

			while(!asyncOperation.isDone){
				if (!progress.IsNone) {
					progress.Value = asyncOperation.progress;
				}
				yield return null;
			}

			if (!loadedEvent.IsNone && !string.IsNullOrEmpty (loadedEvent.Value)) {
				this.Root.Owner.SendEvent (loadedEvent.Value, null);
			}
			Finish ();
		}

	}
}