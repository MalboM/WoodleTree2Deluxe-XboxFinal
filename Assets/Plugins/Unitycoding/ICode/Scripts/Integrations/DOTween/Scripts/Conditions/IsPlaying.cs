#if DOTWEEN
using UnityEngine;
using System;
using DG.Tweening;

namespace ICode.Conditions.DOTweenEngine{
	[Category("DOTween")]  
	[System.Serializable]
	public class IsPlaying : Condition {
		[Shared]
		public FsmTweener tweener;
		public FsmBool equals;

		public override bool Validate ()
		{
			return (tweener.Value != null && tweener.Value.IsPlaying() == equals.Value);
		}
	}
}
#endif