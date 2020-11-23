#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Sends a variable to all clients.")]
	[System.Serializable]
	public class SendToAll : StateAction {
		public FsmInt msgType;
		[ParameterType]
		[Tooltip("Variable to send.")]
		public FsmVariable variable;

		public override void OnEnter ()
		{
			base.OnEnter ();
			FsmVariableMessage message = new FsmVariableMessage (variable);
			NetworkServer.SendToAll ((short)msgType.Value, message);
			Finish ();
		}

	}
}
#endif