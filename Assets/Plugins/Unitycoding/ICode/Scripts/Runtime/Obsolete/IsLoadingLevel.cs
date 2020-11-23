using UnityEngine;
using System.Collections;
using System;
namespace ICode.Conditions{
	[Obsolete("This action becomes obsolete in Unity 5.2, please use SceneManagement.LoadSceneAsync with a custom event to check if loading is done.")]
	[Category(Category.Application)]    
	[Tooltip("Is some level being loaded?")] 
	[HelpUrl("https://docs.unity3d.com/Documentation/ScriptReference/Application-isLoadingLevel.html")]
	[System.Serializable]
	public class IsLoadingLevel : Condition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
			#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
			return Application.isLoadingLevel== equals.Value;
			#else
			return false;
			#endif
		}
	}
}