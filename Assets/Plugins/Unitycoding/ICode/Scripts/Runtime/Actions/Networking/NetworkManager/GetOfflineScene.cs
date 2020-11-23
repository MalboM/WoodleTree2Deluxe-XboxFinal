#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The scene to switch to when offline.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-offlineScene.html")]
	[System.Serializable]
	public class GetOfflineScene : NetworkManagerAction {
		[Shared]
		[Tooltip("Store the value.")]
		public FsmString store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value=manager.offlineScene;
			Finish ();
		}
	}
}

#endif