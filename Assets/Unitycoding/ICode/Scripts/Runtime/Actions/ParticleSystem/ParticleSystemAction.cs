using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityParticleSystem{
	[System.Serializable]
	public abstract class ParticleSystemAction : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;
		
		protected ParticleSystem particleSystem;
		
		public override void OnEnter ()
		{
			particleSystem = gameObject.Value.GetComponent<ParticleSystem> ();
		}
	}
}