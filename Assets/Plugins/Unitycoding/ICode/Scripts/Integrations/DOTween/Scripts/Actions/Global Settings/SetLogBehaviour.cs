#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Sets the LogBehaviour.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetLogBehaviour : StateAction {
		[Tooltip("Depending on the chosen mode DOTween will log only errors, errors and warnings, or everything plus additional informations.")]
		public LogBehaviour logBehaviour;
		public override void OnEnter ()
		{
			DOTween.logBehaviour = logBehaviour;
			Finish ();
		}
		
	}
}
#endif