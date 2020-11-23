#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("Starts a new server.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.StartServer.html")]
	[System.Serializable]
	public class StartServer : NetworkManagerAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.StartServer ();
			Finish ();
		}
	}
}
#endif