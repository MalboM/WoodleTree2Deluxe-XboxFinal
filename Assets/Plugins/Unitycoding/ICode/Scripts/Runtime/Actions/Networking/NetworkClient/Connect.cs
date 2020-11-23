#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Connect client to a NetworkServer instance.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkClient.Connect.html")]
	[System.Serializable]
	public class Connect : StateAction {
		[Tooltip("Target IP address or hostname.")]
		public FsmString serverIp;
		[Tooltip("Target port number.")]
		public FsmInt serverPort;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkClient client = new NetworkClient ();
			client.Connect (serverIp.Value, serverPort.Value);
			Finish ();
		}
	}
}
#endif