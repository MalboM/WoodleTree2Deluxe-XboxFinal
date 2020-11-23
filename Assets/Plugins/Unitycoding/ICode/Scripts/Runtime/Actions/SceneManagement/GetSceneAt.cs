#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Get the scene at index in the SceneManager's list of added scenes.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetSceneAt.html")]
	[System.Serializable]
	public class GetSceneAt : StateAction {
		[Tooltip("Index of the scene to get. Index must be greater than or equal to 0 and less than SceneManager.sceneCount.")]
		public FsmInt index;
		[Shared]
		[NotRequired]
		[InspectorLabel("Name")]
		[Tooltip("Returns the name of the scene.")]
		public FsmString _name;
		[Shared]
		[NotRequired]
		[Tooltip("Returns true if the scene is loaded.")]
		public FsmBool isLoaded;
		[Shared]
		[NotRequired]
		[Tooltip("Returns the relative path of the scene. Like: \"Assets/MyScenes/MyScene.unity\".")]
		public FsmString path;
		[Shared]
		[NotRequired]
		[Tooltip("The number of root transforms of this scene.")]
		public FsmInt rootCount;

		public override void OnEnter ()
		{
			base.OnEnter ();
			Scene scene = SceneManager.GetSceneAt (index.Value);

			if (!isLoaded.IsNone)
				isLoaded.Value = scene.isLoaded;
			if (!_name.IsNone)
				_name.Value = scene.name;
			if (!path.IsNone)
				path.Value = scene.path;
			if (!rootCount.IsNone)
				rootCount.Value = scene.rootCount;
			
			Finish ();
		}
	}
}
#endif