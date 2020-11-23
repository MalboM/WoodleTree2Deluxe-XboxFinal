using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[Category(Category.ParticleSystem)]
	[Tooltip("Starts the particle system.")]
	[HelpUrl("https://docs.unity3d.com/ScriptReference/ParticleSystem.Play.html")]
	[System.Serializable]
	public class Play : ParticleSystemAction {
		[Tooltip("Play all child particle systems as well.")]
		public FsmBool withChildren;

		public override void OnEnter ()
		{
			base.OnEnter ();
			particleSystem.Play (withChildren.Value);
			Finish ();
		}
	}
}