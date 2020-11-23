#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Tells the server that the client is ready.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkClient.Connect.html")]
	[System.Serializable]
	public class SetReady : StateAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			if (!ClientScene.ready) {
				NetworkClient client = NetworkClient.allClients [0];
				ClientScene.Ready (client.connection);
			}
			Finish ();
		}
	}
}
#endif