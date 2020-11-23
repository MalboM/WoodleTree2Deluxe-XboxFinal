#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweens")]    
	[Tooltip("Tween a Vector2 value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class TweenVector2 : StateAction {
		[Shared]
		public FsmVector2 value;
		public FsmVector2 to;
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