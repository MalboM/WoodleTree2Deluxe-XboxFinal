#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Send the tween to the given position in time. ")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Goto : TweenerAction {
		[Tooltip("Time position to reach (if higher than the whole tween duration the tween will simply reach its end). ")]
		public FsmFloat to;
		[Tooltip(" If TRUE the tween will play after reaching the given position, otherwise it will be paused.")]
		public FsmBool play;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.Goto(to.Value,play.Value);
			}
			Finish ();
		}
		
	}
}
#endif