#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global")]    
	[Tooltip("Kills all tweens, clears all pools, resets the max Tweeners/Sequences capacities to the default values.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Clear : StateAction {
		[Tooltip("If TRUE also destroys DOTween's gameObject and resets its initializiation, default settings and everything else (so that next time you use it it will need to be re-initialized).")]
		public FsmBool destroyAll;

		public override void OnEnter ()
		{
			DOTween.Clear (destroyAll);
			Finish ();
		}

	}
}
#endif