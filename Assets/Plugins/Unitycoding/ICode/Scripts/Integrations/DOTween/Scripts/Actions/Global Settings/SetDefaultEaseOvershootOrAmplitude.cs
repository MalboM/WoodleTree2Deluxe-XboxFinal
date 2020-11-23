#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default overshoot/amplitude used for eases.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultEaseOvershootOrAmplitude : StateAction {
		[DefaultValue(1.70158f)]
		[Tooltip("Value to set.")]
		public FsmFloat value;

		public override void OnEnter ()
		{
			DOTween.defaultEaseOvershootOrAmplitude = value.Value;
			Finish ();
		}
		
	}
}
#endif