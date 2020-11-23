#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Sends a tween to its end position (has no effect with tweens that have infinite loops).")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Complete : TweenerAction {

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.Complete();
			}
			Finish ();
		}
		
	}
}
#endif