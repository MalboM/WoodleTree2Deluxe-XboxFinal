#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("This set the address of the matchmaker service.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.SetMatchHost.html")]
	[System.Serializable]
	public class SetMatchHost : NetworkManagerAction {
		[Tooltip("Hostname of matchmaker service.")]
		public FsmString host;
		[Tooltip("Port of matchmaker service.")]
		public FsmInt port;
		[Tooltip("Protocol used by matchmaker service.")]
		public FsmBool https;

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.SetMatchHost (host.Value, port.Value, https.Value);
			Finish ();
		}
	}
}
#endif