using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[Category(Category.ParticleSystem)]
	[Tooltip("Remove all particles in the particle system.")]
	[HelpUrl("https://docs.unity3d.com/ScriptReference/ParticleSystem.Clear.html")]
	[System.Serializable]
	public class Clear : ParticleSystemAction {
		[Tooltip("Clear all child particle systems as well.")]
		public FsmBool withChildren;

		public override void OnEnter ()
		{
			base.OnEnter ();
			particleSystem.Clear (withChildren.Value);
			Finish ();
		}
	}
}