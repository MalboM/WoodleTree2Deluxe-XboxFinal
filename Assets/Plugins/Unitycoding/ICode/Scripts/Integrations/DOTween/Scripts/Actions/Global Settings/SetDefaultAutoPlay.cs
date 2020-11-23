#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default autoPlay behaviour applied to all newly created tweens.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultAutoPlay : StateAction {
		[Tooltip("Value to set.")]
		public AutoPlay autoPlay;
		public override void OnEnter ()
		{
			DOTween.defaultAutoPlay = autoPlay;
			Finish ();
		}
		
	}
}
#endif