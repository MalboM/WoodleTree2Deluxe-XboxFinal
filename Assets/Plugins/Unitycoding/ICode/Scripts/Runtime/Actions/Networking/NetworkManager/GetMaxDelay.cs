#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The maximum delay before sending packets on connections.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-maxDelay.html")]
	[System.Serializable]
	public class GetMaxDelay : NetworkManagerAction {
		[Shared]
		[Tooltip("Store the value.")]
		public FsmFloat store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value=manager.maxDelay;
			Finish ();
		}
	}
}
#endif