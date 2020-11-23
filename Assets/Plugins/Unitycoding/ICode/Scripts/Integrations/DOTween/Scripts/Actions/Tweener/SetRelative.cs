#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Tween a color value.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetRelative : TweenerAction {
		public FsmBool value;
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.SetRelative(value.Value);
			}
			Finish ();
		}
		
	}
}
#endif