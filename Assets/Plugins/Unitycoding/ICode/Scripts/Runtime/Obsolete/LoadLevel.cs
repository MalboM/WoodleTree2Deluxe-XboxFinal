using UnityEngine;
using System.Collections;
using System;

namespace ICode.Actions.UnityApplication
{
	[Obsolete ("This action is obsolete. Please use SceneManagement.LoadScene")]
	[Category (Category.Application)]
	[Tooltip ("Loads the level by its name.")]
	[HelpUrl ("https://docs.unity3d.com/Documentation/ScriptReference/Application.LoadLevel.html")]
	[System.Serializable]
	public class LoadLevel : StateAction
	{
		[Tooltip ("The name of the level to load.")]
		public FsmString level;

		public override void OnEnter ()
		{
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 
			Application.LoadLevel (level.Value);
			#else
			UnityEngine.SceneManagement.SceneManager.LoadScene (level.Value);
			#endif
			Finish ();
		}
	}
}