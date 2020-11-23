#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Tweener")]    
	[Tooltip("Forces the tween to initialize its settings immediately.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class ForceInit : TweenerAction {

		public override void OnEnter ()
		{
			if (tweener.Value != null) {
				tweener.Value.ForceInit();
			}
			Finish ();
		}
		
	}
}
#endif