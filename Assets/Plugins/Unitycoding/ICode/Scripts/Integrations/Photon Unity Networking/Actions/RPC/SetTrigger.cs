#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.CustomRPC{
	[Category("Photon/RPC")]
	[Tooltip("SetTrigger over network.")]
	[System.Serializable]
	public class SetTrigger : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Trigger name set in AnimatorController.")]
		public FsmString trigger;
		public PhotonTargets targets;
		
		private PhotonView photonView;
		
		public override void OnEnter ()
		{
			photonView = PhotonView.Get (gameObject.Value);
			photonView.RPC ("SetTrigger", targets, trigger.Value);
			Finish ();
			
		}
	}
}
#endif