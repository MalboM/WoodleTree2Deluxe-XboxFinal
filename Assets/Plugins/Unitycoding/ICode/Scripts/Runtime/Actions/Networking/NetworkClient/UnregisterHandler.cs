#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking.Client{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Unregisters a network message handler.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkClient.UnregisterHandler.html")]
	[System.Serializable]
	public class UnregisterHandler : StateAction {
		public FsmInt msgType;

		public override void OnEnter ()
		{
			base.OnEnter ();
			if (NetworkClient.allClients.Count > 0) {
				NetworkClient client=NetworkClient.allClients[0];
				client.UnregisterHandler((short)msgType.Value);
			}
			Finish ();
		}
	}
}
#endif