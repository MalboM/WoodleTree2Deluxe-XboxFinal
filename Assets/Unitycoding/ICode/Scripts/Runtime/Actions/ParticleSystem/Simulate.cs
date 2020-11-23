using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[Category(Category.ParticleSystem)]
	[Tooltip("Fastforwards the particle system by simulating particles over given period of time, then pauses it.")]
	[HelpUrl("https://docs.unity3d.com/ScriptReference/ParticleSystem.Simulate.html")]
	[System.Serializable]
	public class Simulate : ParticleSystemAction {
		[Tooltip("Time to fastforward the particle system.")]
		public FsmFloat time;
		[Tooltip("Play all child particle systems as well.")]
		public FsmBool withChildren;
		[Tooltip("Restart and start from the beginning.")]
		public FsmBool restart;

		public override void OnEnter ()
		{
			base.OnEnter ();
			particleSystem.Simulate (time.Value,withChildren.Value,restart.Value);
			Finish ();
		}
	}
}