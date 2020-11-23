using UnityEngine;
using System.Collections;
using System;

namespace ICode.Actions.UnityApplication{
	[Obsolete("This action is obsolete. Please use SceneManagement.LoadSceneAsync")]
	[Category(Category.Application)]
	[Tooltip("Loads the level asynchronously in the background.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Application.LoadLevelAsync.html")]
	[System.Serializable]
	public class LoadLevelAsync : StateAction {
		[Tooltip("The name of the level to load.")]
		public FsmString level;
		[NotRequired]
		[Tooltip("Send event when loading is done. Check with OnCustomEvent")]
		public FsmString loadedEvent;
		[NotRequired]
		[Shared]
		[Tooltip("Store the progress of loading.")]
		public FsmFloat progress;
		
		private AsyncOperation asyncOperation;
		
		public override void OnUpdate ()
		{
			if (asyncOperation == null) {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				asyncOperation = Application.LoadLevelAsync (level.Value);
				#else
				asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level.Value); 
				#endif
				asyncOperation.allowSceneActivation = false;
			}else{
				if (!progress.IsNone) {
					progress.Value = asyncOperation.progress;
				}
				asyncOperation.allowSceneActivation=!(asyncOperation.progress<0.9f);
				
				if (asyncOperation.isDone) {
					if (!loadedEvent.IsNone && !string.IsNullOrEmpty (loadedEvent.Value)) {
						this.Root.Owner.SendEvent (loadedEvent.Value, null);
					}
					Finish ();
				}
			}
		}
	}
}