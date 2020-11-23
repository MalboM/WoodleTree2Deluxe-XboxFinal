#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("This will merge the source scene into the destinationScene. This function merges the contents of the source scene into the destination scene, and deletes the source scene. All GameObjects at the root of the source scene are moved to the root of the destination scene. NOTE: This function is destructive: The source scene will be destroyed once the merge has been completed.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.MergeScenes.html")]
	[System.Serializable]
	public class MergeScenes : StateAction {
		[NotRequired]
		[Tooltip("The index of the scene that will be merged into the destination scene.")]
		public FsmInt sourceSceneIndex;
		[NotRequired]
		[Tooltip("The name of the scene that will be merged into the destination scene.")]
		public FsmString sourceSceneName;

		[NotRequired]
		[Tooltip("Existing scene index to merge the source scene into.")]
		public FsmInt destinationSceneIndex;
		[NotRequired]
		[Tooltip("Existing scene name to merge the source scene into.")]
		public FsmString destinationSceneName;

		public override void OnEnter ()
		{
			base.OnEnter ();
			Scene sourceScene = GetScene(sourceSceneName,sourceSceneIndex);
			Scene destinationScene = GetScene (destinationSceneName, destinationSceneIndex);
			if (destinationScene.IsValid ()) {
				SceneManager.MergeScenes (sourceScene, destinationScene);
			} else {
				Debug.LogWarning ("Could not merge scenes. The destination scene is invalid!");
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