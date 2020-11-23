using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[Category(Category.ParticleSystem)]
	[Tooltip("Stops playing the particle system.")]
	[HelpUrl("https://docs.unity3d.com/ScriptReference/ParticleSystem.Stop.html")]
	[System.Serializable]
	public class Stop : ParticleSystemAction {
		[Tooltip("Stop all child particle systems as well.")]
		public FsmBool withChildren;

		public override void OnEnter ()
		{
			base.OnEnter ();
			particleSystem.Stop (withChildren.Value);
			Finish ();
		}
	}
}