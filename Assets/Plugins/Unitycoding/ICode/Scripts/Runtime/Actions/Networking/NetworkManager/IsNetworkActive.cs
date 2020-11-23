#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("True if the NetworkServer or NetworkClient is active.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-isNetworkActive.html")]
	[System.Serializable]
	public class IsNetworkActive : NetworkManagerAction {
		[Shared]
		[Tooltip("Store the value.")]
		public FsmBool store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value=manager.isNetworkActive;
			Finish ();
		}
	}
}
#endif