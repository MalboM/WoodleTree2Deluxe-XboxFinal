#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweens")]    
	[Tooltip("Tween a Vector3 value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class TweenVector3 : StateAction {
		[Shared]
		public FsmVector3 value;
		public FsmVector3 to;
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