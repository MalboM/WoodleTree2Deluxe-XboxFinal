#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default period used for eases.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultEasePeriod : StateAction {
		[Tooltip("Value to set.")]
		public FsmFloat value;

		public override void OnEnter ()
		{
			DOTween.defaultEasePeriod = value.Value;
			Finish ();
		}
		
	}
}
#endif