#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The scene to switch to when online.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-onlineScene.html")]
	[System.Serializable]
	public class GetOnlineScene : NetworkManagerAction {
		[Shared]
		[Tooltip("Store the value.")]
		public FsmString store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value=manager.onlineScene;
			Finish ();
		}
	}
}
#endif