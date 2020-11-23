#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.CustomRPC{
	[Category("Photon/RPC")]
	[Tooltip("Sends a custom RPC with one paramtere.")]
	[System.Serializable]
	public class SendRPC1 : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The name of the method.")]
		public FsmString methodName;
		[Tooltip("Parameter 1 to send.")]
		[ParameterType]
		public FsmVariable parameter1;
		public PhotonTargets targets;
		
		private PhotonView photonView;
		
		public override void OnEnter ()
		{
			Debug.Log (this.Root.Name);
			photonView = PhotonView.Get (gameObject.Value);
			photonView.RPC (methodName.Value, targets, parameter1.GetValue ());
			Finish ();
			
		}
	}
}
#endif