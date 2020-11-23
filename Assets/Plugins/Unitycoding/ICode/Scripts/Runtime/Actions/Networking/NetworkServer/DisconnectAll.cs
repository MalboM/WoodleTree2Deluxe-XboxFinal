#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Disconnect all currently connected clients.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.DisconnectAll.html")]
	[System.Serializable]
	public class DisconnectAll : StateAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkServer.DisconnectAll ();
			Finish ();
		}
	}
}
#endif