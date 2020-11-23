#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Sets the looping options")]
	[System.Serializable]
	public class SetLoops : TweenerAction {
		[Tooltip("Setting loops to -1 will make the tween loop infinitely.")]
		public FsmInt loops;
		public LoopType loopType;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetLoops (loops.Value, loopType);
			}
			Finish ();
		}

	}
}
#endif