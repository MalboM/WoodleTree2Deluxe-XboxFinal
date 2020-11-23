#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweens")]    
	[Tooltip("Tween a float value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class TweenFloat : StateAction {
		[Shared]
		public FsmFloat value;
		public FsmFloat to;
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