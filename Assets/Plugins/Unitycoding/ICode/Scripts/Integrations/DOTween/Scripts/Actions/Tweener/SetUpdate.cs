#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Sets the type of update (Normal, Late or Fixed) for the tween and eventually tells it to ignore Unity's timeScale. ")]
	[System.Serializable]
	public class SetUpdate : TweenerAction {
		public UpdateType updateType;
		[Tooltip("If TRUE the tween will ignore Unity's Time.timeScale. ")]
		public FsmBool isIndependentUpdate;

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetUpdate (updateType, isIndependentUpdate.Value);
			}
			Finish ();
		}

	}
}
#endif