#if DOTWEEN
using UnityEngine;
using System;
using DG.Tweening;

namespace ICode.Conditions.DOTweenEngine{
	[Category("DOTween")]  
	[Tooltip("Sets a callback that will be fired when the tween is rewinded, either by calling Rewind or by reaching the start position while playing backwards. NOTE: Rewinding a tween that is already rewinded will not fire this callback.")]
	[System.Serializable]
	public class OnRewind : Condition {
		[Shared]
		public FsmTweener tweener;
		private bool isTrigger;
		
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.OnRewind (OnTrigger);
			} else {
				tweener.onVariableChange.AddListener(OnVariableChange);
			}
		}
		
		public override void OnExit ()
		{
			tweener.onVariableChange.RemoveListener (OnVariableChange);
			isTrigger = false;
		}

		private void OnVariableChange(object value){
			if (tweener.Value != null) {
				tweener.Value.OnRewind (OnTrigger);
			}
		}

		private void OnTrigger(){
			isTrigger = true;
		}
		
		public override bool Validate ()
		{
			if (isTrigger) {
				isTrigger=false;	
				return true;
			}
			return isTrigger;
		}
	}
}
#endif