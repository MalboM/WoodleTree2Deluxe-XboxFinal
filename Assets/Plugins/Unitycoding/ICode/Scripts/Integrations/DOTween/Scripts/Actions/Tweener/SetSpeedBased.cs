#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Tween a color value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetSpeedBased : TweenerAction {
		public FsmBool value;
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetSpeedBased(value.Value);
			}
			Finish ();
		}
		
	}
}
#endif