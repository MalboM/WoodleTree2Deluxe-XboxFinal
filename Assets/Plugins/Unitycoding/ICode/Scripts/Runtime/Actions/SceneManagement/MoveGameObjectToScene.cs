#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Move a GameObject from its current scene to a new scene. It is required that the GameObject is at the root of its current scene.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.MoveGameObjectToScene.html")]
	[System.Serializable]
	public class MoveGameObjectToScene : StateAction {
		[NotRequired]
		[Tooltip("Scene index to move into.")]
		public FsmInt sceneIndex;
		[NotRequired]
		[Tooltip("Scene name to move into.")]
		public FsmString sceneName;

		[SharedPersistent]
		[Tooltip("GameObject to move.")]
		public FsmGameObject gameObject;

		public override void OnEnter ()
		{
			base.OnEnter ();
			Scene scene = GetScene(sceneName,sceneIndex);
			if (scene.IsValid () && gameObject.Value != null) {
				SceneManager.MoveGameObjectToScene (gameObject.Value, scene);
			} else {
				Debug.LogWarning ("Failed to move game object!");
			}
			Finish ();
		}

		private Scene GetScene(FsmString sceneName,FsmInt sceneIndex){
			if (!sceneName.IsNone) {
				return SceneManager.GetSceneByName (sceneName.Value);
			}
			return SceneManager.GetSceneAt (sceneIndex.Value);
		}
	}
}
#endif