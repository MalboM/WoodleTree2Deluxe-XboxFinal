#if DOTWEEN
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace ICode.Actions.DOTweenEngine{
	[Category("DOTween/Global")]    
	[Tooltip("Initializes DOTween.")]
	[HelpUrl("http://dotween.demigiant.com/documentation.php")]
	[System.Serializable]
	public class Init : StateAction {
		[Tooltip("If TRUE all new tweens will be set for recycling, meaning that when killed they won't be destroyed but instead will be put in a pool and reused rather than creating new tweens.")]
		public FsmBool recycleAllByDefault;
		[Tooltip("If set to TRUE tweens will be slightly slower but safer, allowing DOTween to automatically take care of things like targets being destroyed while a tween is running.")]
		public FsmBool useSafeMode;
		[Tooltip("Depending on the chosen mode DOTween will log only errors, errors and warnings, or everything plus additional informations.")]
		public LogBehaviour logBehaviour;
		public override void OnEnter ()
		{
			DOTween.Init (recycleAllByDefault.Value,useSafeMode.Value,logBehaviour);
			Finish ();
		}

	}
}
#endif