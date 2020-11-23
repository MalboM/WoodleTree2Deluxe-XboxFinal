#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.CustomRPC{
	[Category("Photon/RPC")]
	[Tooltip("Sends a custom RPC with two paramteres")]
	[System.Serializable]
	public class SendRPC2 : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The name of the method.")]
		public FsmString methodName;
		[Tooltip("Parameter 1 to send.")]
		[ParameterType]
		public FsmVariable parameter1;
		[Tooltip("Parameter 2 to send.")]
		[ParameterType]
		public FsmVariable parameter2;
		public PhotonTargets targets;
		
		private PhotonView photonView;
		
		public override void OnEnter ()
		{
			photonView = PhotonView.Get (gameObject.Value);
			photonView.RPC (methodName.Value, targets, parameter1.GetValue (),parameter2.GetValue());
			Finish ();
			
		}
	}
}
#endif