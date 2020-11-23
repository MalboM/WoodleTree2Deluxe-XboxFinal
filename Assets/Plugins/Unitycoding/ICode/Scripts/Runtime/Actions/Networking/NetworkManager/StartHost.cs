#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("This starts a network host - a server and client in the same application.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.StartHost.html")]
	[System.Serializable]
	public class StartHost : NetworkManagerAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.StartHost ();
			Finish ();
		}
	}
}
#endif