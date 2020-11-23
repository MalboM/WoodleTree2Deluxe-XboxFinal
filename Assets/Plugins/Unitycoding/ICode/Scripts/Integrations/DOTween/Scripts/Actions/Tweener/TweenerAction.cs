#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[System.Serializable]
	public abstract class TweenerAction : StateAction {
		[Shared]
		public FsmTweener tweener;
		
	}
}
#endif