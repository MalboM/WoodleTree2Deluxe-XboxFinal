#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Pauses the tween.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Pause : TweenerAction {
	
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.Pause();
			}
			Finish ();
		}
		
	}
}
#endif