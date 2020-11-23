#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global")]    
	[Tooltip("Clears all cached tween pools.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class ClearCachedTweens : StateAction {

		public override void OnEnter ()
		{
			DOTween.ClearCachedTweens ();
			Finish ();
		}

	}
}
#endif