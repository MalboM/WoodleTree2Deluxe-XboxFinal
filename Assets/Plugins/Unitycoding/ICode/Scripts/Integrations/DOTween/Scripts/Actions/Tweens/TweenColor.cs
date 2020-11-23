#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweens")]    
	[Tooltip("Tween a color value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class TweenColor : StateAction {
		[Shared]
		public FsmColor value;
		public FsmColor to;
		public FsmFloat duration;
		[Shared]
		[NotRequired]
		public FsmTweener store;

		public override void OnEnter ()
		{
			Tweener tweener=DOTween.To (() => value.Value, x => value.Value = x, to.Value, duration.Value);
			store.Value = tweener;
			Finish ();
		}

	}
}
#endif