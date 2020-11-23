#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Plays the tween.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Play : TweenerAction {
	
		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.Play();
			}
			Finish ();
		}
		
	}
}
#endif