#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Plays the tween if it was paused, pauses it if it was playing.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class TogglePause : TweenerAction {

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.TogglePause();
			}
			Finish ();
		}
		
	}
}
#endif