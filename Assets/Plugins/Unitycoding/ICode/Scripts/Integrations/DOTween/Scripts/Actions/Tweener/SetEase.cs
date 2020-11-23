#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Sets the ease of the tween.")]
	[System.Serializable]
	public class SetEase : TweenerAction {
		public Ease value;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetEase (value);
			}
			Finish ();
		}

	}
}
#endif