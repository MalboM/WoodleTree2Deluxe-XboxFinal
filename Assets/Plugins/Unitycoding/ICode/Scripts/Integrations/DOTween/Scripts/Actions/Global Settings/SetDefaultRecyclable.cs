#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default recycling behaviour applied to all newly created tweens.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultRecyclable : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;

		public override void OnEnter ()
		{
			DOTween.defaultRecyclable = value.Value;
			Finish ();
		}
		
	}
}
#endif