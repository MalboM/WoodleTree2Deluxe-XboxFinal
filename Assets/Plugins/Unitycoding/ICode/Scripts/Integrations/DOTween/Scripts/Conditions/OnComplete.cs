#if DOTWEEN
using UnityEngine;
using System;
using DG.Tweening;

namespace ICode.Conditions.DOTweenEngine{
	[Category("DOTween")]  
	[Tooltip("Sets a callback that will be fired the moment the tween reaches completion, all loops included.")]
	[System.Serializable]
	public class OnComplete : Condition {
		[Shared]
		public FsmTweener tweener;
		private bool isTrigger;
		
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.OnComplete (OnTrigger);
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
				tweener.Value.OnComplete (OnTrigger);
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