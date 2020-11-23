#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("Default UpdateType applied to all newly created tweens.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetDefaultUpdateType : StateAction {
		[Tooltip("Value to set.")]
		public UpdateType updateType;

		public override void OnEnter ()
		{
			DOTween.defaultUpdateType = updateType;
			Finish ();
		}
		
	}
}
#endif