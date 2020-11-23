#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Set the scene to be active.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.SetActiveScene.html")]
	[System.Serializable]
	public class SetActiveScene : StateAction {
		[NotRequired]
		[Tooltip("The index of the scene to set active.")]
		public FsmInt index;
		[NotRequired]
		[InspectorLabel("Name")]
		[Tooltip("The name of the scene to set active.")]
		public FsmString _name;

		public override void OnEnter ()
		{
			base.OnEnter ();
			Scene scene = GetScene (_name,index);
			if (scene.IsValid ()) {
				SceneManager.SetActiveScene (scene);
			} else {
				Debug.LogWarning ("Scene is invalid!");
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