#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global Settings")]    
	[Tooltip("In order to be faster DOTween limits the max amount of active tweens you can have. If you go beyond that limit don't worry: it is automatically increased. Still, if you already know you'll need more (or less) than the default max Tweeners/Sequences (which is 200 Tweeners and 50 Sequences) you can set DOTween's capacity at startup and avoid hiccups when it's raised automatically.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class SetTweensCapacity : StateAction {
		public FsmInt maxTweeners;
		public FsmInt maxSequences;
		public override void OnEnter ()
		{
			DOTween.SetTweensCapacity (maxTweeners.Value, maxSequences.Value);;
			Finish ();
		}
		
	}
}
#endif