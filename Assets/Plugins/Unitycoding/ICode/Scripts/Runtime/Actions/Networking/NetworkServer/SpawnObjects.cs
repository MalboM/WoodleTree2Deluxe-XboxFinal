#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("This causes NetworkIdentity objects in a scene to be spawned on a server.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.SpawnObjects.html")]
	[System.Serializable]
	public class SpawnObjects : StateAction {
		[Shared]
		[NotRequired]
		[Tooltip("Success if objects where spawned.")]
		public FsmBool result;

		public override void OnEnter ()
		{
			base.OnEnter ();
			result.Value=NetworkServer.SpawnObjects ();
			Finish ();
		}
	}
}
#endif