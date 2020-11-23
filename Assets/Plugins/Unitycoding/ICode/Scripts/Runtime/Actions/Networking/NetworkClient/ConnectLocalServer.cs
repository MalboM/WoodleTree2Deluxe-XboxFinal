#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Create and connect a local client instance to the local server.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.ClientScene.ConnectLocalServer.html")]
	[System.Serializable]
	public class ConnectLocalServer : StateAction {
		[Shared]
		[NotRequired]
		[Tooltip("Created connection id.")]
		public FsmInt connectionId;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkClient client = ClientScene.ConnectLocalServer ();
			connectionId.Value = client.connection.connectionId;
			Finish ();
		}
	}
}
#endif