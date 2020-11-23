#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default loop type applied to all newly created tweens that involve loops.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultLoopType : StateAction {
		[Tooltip("Value to set.")]
		public LoopType loopType;

		public override void OnEnter ()
		{
			DOTween.defaultLoopType = loopType;
			Finish ();
		}
		
	}
}
#endif