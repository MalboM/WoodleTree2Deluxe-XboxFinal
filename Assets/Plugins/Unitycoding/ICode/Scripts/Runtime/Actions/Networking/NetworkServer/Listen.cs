#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Start the server on the given port number. Note that if a UMatch has been created, this will list using the relay server in the UMatch.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.Listen.html")]
	[System.Serializable]
	public class Listen : StateAction {
		[Tooltip("Listen port number.")]
		public FsmInt serverPort;
		[Shared]
		[NotRequired]
		public FsmBool result;

		public override void OnEnter ()
		{
			base.OnEnter ();
			result.Value=NetworkServer.Listen (serverPort.Value);
			Finish ();
		}
	}
}
#endif