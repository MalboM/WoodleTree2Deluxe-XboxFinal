#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Plays the tween backwards.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class PlayBackwards : TweenerAction {
	
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.PlayBackwards();
			}
			Finish ();
		}
		
	}
}
#endif