#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The network address currently in use.For clients, this is the address of the server that is connected to. For servers, this is the local address.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-networkAddress.html")]
	[System.Serializable]
	public class SetNetworkAddress : NetworkManagerAction {
		[Tooltip("Value to set.")]
		public FsmString value;

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.networkAddress=value.Value;
			Finish ();
		}
	}
}
#endif