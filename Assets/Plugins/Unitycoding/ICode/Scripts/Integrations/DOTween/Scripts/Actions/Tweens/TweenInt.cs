#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweens")]    
	[Tooltip("Tween an int value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class TweenInt : StateAction {
		[Shared]
		public FsmInt value;
		public FsmInt to;
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