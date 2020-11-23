#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("If set to TRUE you will get a DOTween report when exiting play mode (only in the Editor). Useful to know how many max Tweeners and Sequences you reached and optimize your final project accordingly. Beware, this will slightly slow down your performance while inside Unity Editor.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class ShowUnityEditorReport : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;
		public override void OnEnter ()
		{
			DOTween.showUnityEditorReport = value.Value;
			Finish ();
		}
		
	}
}
#endif