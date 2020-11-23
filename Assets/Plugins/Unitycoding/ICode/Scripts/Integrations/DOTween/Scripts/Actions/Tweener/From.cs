#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Changes the Tweener to a FROM tween (instead of a regular TO tween), immediately sending the target to its given value and then tweening to what was its previous value. ")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class From : TweenerAction {
		public FsmBool isRelative;
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.From(isRelative.Value);
			}
			Finish ();
		}
		
	}
}
#endif