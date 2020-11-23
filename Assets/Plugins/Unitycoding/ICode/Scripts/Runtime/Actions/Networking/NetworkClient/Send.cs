#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Sends a network message to the connected server. The contents can be a message object - which will be automatically serialized, or it can be a MemoryStream containing serialized data.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkClient.Send.html")]
	[System.Serializable]
	public class Send : StateAction {
		public FsmInt msgType;
		[ParameterType]
		[Tooltip("Variable to send.")]
		public FsmVariable variable;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (NetworkClient.allClients.Count > 0) {
				NetworkClient client = NetworkClient.allClients [0];
				FsmVariableMessage message = new FsmVariableMessage (variable);
				client.Send((short)msgType.Value,message);
			}
			Finish ();
		}
	}
}
#endif