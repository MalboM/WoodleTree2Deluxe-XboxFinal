#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("Stops the server that the manager is using.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.StopServer.html")]
	[System.Serializable]
	public class StopServer : NetworkManagerAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.StopServer ();
			Finish ();
		}
	}
}
#endif