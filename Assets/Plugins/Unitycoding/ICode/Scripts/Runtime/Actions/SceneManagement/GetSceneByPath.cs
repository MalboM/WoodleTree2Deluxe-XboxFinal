#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Searches all scenes added to the SceneManager for a scene that has the given asset path.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetSceneByPath.html")]
	[System.Serializable]
	public class GetSceneByPath : StateAction {
		[Tooltip("Path of the scene. Should be relative to the project folder. Like: \"Assets/MyScenes/MyScene.unity\".")]
		public FsmString path;
		[Shared]
		[NotRequired]
		[Tooltip("Returns the index of the scene in the Build Settings. Always returns -1 if the scene was loaded through an AssetBundle.")]
		public FsmInt buildIndex;
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
		[Tooltip("The number of root transforms of this scene.")]
		public FsmInt rootCount;

		public override void OnEnter ()
		{
			base.OnEnter ();
			Scene scene = SceneManager.GetSceneByPath (path.Value);

			if (!buildIndex.IsNone)
				buildIndex.Value = scene.buildIndex;
			if (!isLoaded.IsNone)
				isLoaded.Value = scene.isLoaded;
			if (!_name.IsNone)
				_name.Value = scene.name;
			if (!rootCount.IsNone)
				rootCount.Value = scene.rootCount;
			
			Finish ();
		}
	}
}
#endif