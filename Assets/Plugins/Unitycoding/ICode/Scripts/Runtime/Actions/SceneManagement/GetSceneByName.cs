#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Searches through the scenes added to the SceneManager for a scene with the given name.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetSceneByName.html")]
	[System.Serializable]
	public class GetSceneByName : StateAction {
		[InspectorLabel("Name")]
		[Tooltip("Name of scene to find.")]
		public FsmString _name;
		[Shared]
		[NotRequired]
		[Tooltip("Returns the index of the scene in the Build Settings. Always returns -1 if the scene was loaded through an AssetBundle.")]
		public FsmInt buildIndex;
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
			Scene scene = SceneManager.GetSceneByName (_name.Value);

			if (!buildIndex.IsNone)
				buildIndex.Value = scene.buildIndex;
			if (!isLoaded.IsNone)
				isLoaded.Value = scene.isLoaded;
			if (!path.IsNone)
				path.Value = scene.path;
			if (!rootCount.IsNone)
				rootCount.Value = scene.rootCount;
			
			Finish ();
		}
	}
}
#endif