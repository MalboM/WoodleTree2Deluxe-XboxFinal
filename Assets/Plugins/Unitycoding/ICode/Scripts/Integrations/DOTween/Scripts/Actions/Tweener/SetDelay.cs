#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Sets a delayed startup for the tween. ")]
	[System.Serializable]
	public class SetDelay : TweenerAction {
		public FsmFloat value;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetDelay (value.Value);
			}
			Finish ();
		}

	}
}
#endif