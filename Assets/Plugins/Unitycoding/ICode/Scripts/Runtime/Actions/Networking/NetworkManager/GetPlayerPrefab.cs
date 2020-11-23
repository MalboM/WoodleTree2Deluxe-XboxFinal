#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The default prefab to be used to create player objects on the server.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-playerPrefab.html")]
	[System.Serializable]
	public class GetPlayerPrefab : NetworkManagerAction {
		[Shared]
		[Tooltip("Store the value.")]
		public FsmGameObject store;

		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value=manager.playerPrefab;
			Finish ();
		}
	}
}
#endif