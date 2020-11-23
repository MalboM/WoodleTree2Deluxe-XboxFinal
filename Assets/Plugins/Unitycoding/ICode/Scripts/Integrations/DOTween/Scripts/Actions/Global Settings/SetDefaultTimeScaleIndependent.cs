#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Sets whether Unity's timeScale should be taken into account by default or not.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultTimeScaleIndependent : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;

		public override void OnEnter ()
		{
			DOTween.defaultTimeScaleIndependent = value.Value;
			Finish ();
		}
		
	}
}
#endif