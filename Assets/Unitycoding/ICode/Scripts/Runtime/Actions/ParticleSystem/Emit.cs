using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[Category(Category.ParticleSystem)]
	[Tooltip("Emit count particles immediately.")]
	[HelpUrl("https://docs.unity3d.com/ScriptReference/ParticleSystem.Emit.html")]
	[System.Serializable]
	public class Emit : ParticleSystemAction {
		[Tooltip("Number of particles to emit.")]
		public FsmInt count;

		public override void OnEnter ()
		{
			base.OnEnter ();
			particleSystem.Emit (count.Value);
			Finish ();
		}
	}
}