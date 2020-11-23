#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("If set to TRUE tweens will be slightly slower but safer, allowing DOTween to automatically take care of things like targets being destroyed while a tween is running. Setting it to FALSE means you'll have to personally take care of killing a tween before its target is destroyed or somehow rendered invalid. ")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class UseSafeMode : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;
		public override void OnEnter ()
		{
			DOTween.useSafeMode = value.Value;
			Finish ();
		}
		
	}
}
#endif