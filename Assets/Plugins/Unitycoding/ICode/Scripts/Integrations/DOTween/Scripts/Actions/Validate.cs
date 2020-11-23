#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global")]    
	[Tooltip("Validates all active tweens and removes eventually invalid ones (usually because their target was destroyed). This is a slightly expensive operation so use it with care. Also, no need to use it at all especially if safe mode is ON.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Validate : StateAction {

		public override void OnEnter ()
		{
			DOTween.Validate ();
			Finish ();
		}

	}
}
#endif