#if DOTWEEN
using UnityEngine;
using System;
using DG.Tweening;

namespace ICode.Conditions.DOTweenEngine{
	[Category("DOTween")]  
	[Tooltip("Sets a callback that will be fired when the tween state changes from playing to paused. If the tween has autoKill set to FALSE, this is called also when the tween reaches completion.")]
	[System.Serializable]
	public class OnPause : Condition {
		[Shared]
		public FsmTweener tweener;
		private bool isTrigger;
		
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.OnPause (OnTrigger);
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
				tweener.Value.OnPause (OnTrigger);
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