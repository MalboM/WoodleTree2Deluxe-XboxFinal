using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[Category(Category.ParticleSystem)]
	[Tooltip("Pauses playing the particle system.")]
	[HelpUrl("https://docs.unity3d.com/ScriptReference/ParticleSystem.Pause.html")]
	[System.Serializable]
	public class Pause : ParticleSystemAction {
		[Tooltip("Pause all child particle systems as well.")]
		public FsmBool withChildren;

		public override void OnEnter ()
		{
			base.OnEnter ();
			particleSystem.Pause (withChildren.Value);
			Finish ();
		}
	}
}