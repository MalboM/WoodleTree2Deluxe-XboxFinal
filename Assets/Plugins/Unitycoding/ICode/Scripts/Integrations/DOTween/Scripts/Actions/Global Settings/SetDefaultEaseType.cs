#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default period used for eases.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultEaseType : StateAction {
		[Tooltip("Value to set.")]
		public Ease ease;

		public override void OnEnter ()
		{
			DOTween.defaultEaseType = ease;
			Finish ();
		}
		
	}
}
#endif