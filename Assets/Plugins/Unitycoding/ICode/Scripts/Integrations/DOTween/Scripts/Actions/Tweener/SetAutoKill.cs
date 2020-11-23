#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("If value is set to TRUE the tween will be killed as soon as it completes, otherwise it will stay in memory and you'll be able to reuse it.")]
	[System.Serializable]
	public class SetAutoKill : TweenerAction {
		public FsmBool value;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetAutoKill (value.Value);
			}
			Finish ();
		}

	}
}
#endif