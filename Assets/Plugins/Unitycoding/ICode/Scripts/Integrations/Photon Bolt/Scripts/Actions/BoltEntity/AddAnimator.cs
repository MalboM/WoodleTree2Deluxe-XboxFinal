#if PHOTON_BOLT
using UnityEngine;
using System.Collections;
using Bolt;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Sets the Transform property.")]
	[System.Serializable]
	public class AddAnimator : BoltEntityAction {
		[Tooltip("Value to set.")]
		public FsmObject animator;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			entity.GetState<IState> ().AddAnimator(animator.Value as Animator);
			Finish ();
		}
	}
}
#endif