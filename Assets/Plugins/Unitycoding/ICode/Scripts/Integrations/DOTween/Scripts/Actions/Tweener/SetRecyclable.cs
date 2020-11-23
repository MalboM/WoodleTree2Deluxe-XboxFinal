#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip(" If TRUE the tween will be recycled after being killed, otherwise it will be destroyed.")]
	[System.Serializable]
	public class SetRecyclable : TweenerAction {
		public FsmBool value;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetRecyclable (value.Value);
			}
			Finish ();
		}

	}
}
#endif