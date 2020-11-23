#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Rewind the tween.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Rewind : TweenerAction {
		public FsmBool includeDelay;
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.Rewind(includeDelay.Value);
			}
			Finish ();
		}
		
	}
}
#endif