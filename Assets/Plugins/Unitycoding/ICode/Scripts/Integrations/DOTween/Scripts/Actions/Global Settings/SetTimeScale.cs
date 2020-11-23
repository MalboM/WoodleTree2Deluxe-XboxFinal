#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Global timeScale applied to all tweens, both regular and independent.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetTimeScale : StateAction {
		[Tooltip("Scale value to set.")]
		public FsmFloat timeScale;
		public override void OnEnter ()
		{
			DOTween.timeScale = timeScale.Value;
			Finish ();
		}
		
	}
}
#endif