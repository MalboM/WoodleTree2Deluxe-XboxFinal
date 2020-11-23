#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default autoKill behaviour applied to all newly created tweens.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultAutoKill : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;
		public override void OnEnter ()
		{
			DOTween.defaultAutoKill = value.Value;
			Finish ();
		}
		
	}
}
#endif