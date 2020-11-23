using UnityEngine;
using System.Collections;

#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine.SceneManagement;
#endif

namespace ICode.Actions.UnitySceneManagement{
	[Category(Category.SceneManagement)]
	[Tooltip("Get the active scene.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetActiveScene.html")]
	[System.Serializable]
	public class GetActiveScene : StateAction {
		[Shared]
		[NotRequired]
		[Tooltip("Returns the index of the scene in the Build Settings. Always returns -1 if the scene was loaded through an AssetBundle.")]
		public FsmInt buildIndex;
		[Shared]
		[NotRequired]
		[InspectorLabel("Name")]
		[Tooltip("Returns the name of the scene.")]
		public FsmString _name;

		#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
		[Shared]
		[NotRequired]
		[Tooltip("Returns the relative path of the scene. Like: \"Assets/MyScenes/MyScene.unity\".")]
		public FsmString path;
		[Shared]
		[NotRequired]
		[Tooltip("The number of root transforms of this scene.")]
		public FsmInt rootCount;
		#endif

		public override void OnEnter ()
		{
			base.OnEnter ();
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			if(!_name.IsNone)
				_name.Value = Application.loadedLevelName;
			if(!buildIndex.IsNone)
				buildIndex.Value = Application.loadedLevel;
			#else
			Scene scene = SceneManager.GetActiveScene ();
			if (!buildIndex.IsNone)
				buildIndex.Value = scene.buildIndex;
			if (!_name.IsNone)
				_name.Value = scene.name;
			if (!path.IsNone)
				path.Value = scene.path;
			if (!rootCount.IsNone)
				rootCount.Value = scene.rootCount;
			#endif
			Finish ();
		}
	}
}