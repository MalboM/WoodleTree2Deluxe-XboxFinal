#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Spawn the given game object on all clients which are ready.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.Spawn.html")]
	[System.Serializable]
	public class Spawn : StateAction {
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		public override void OnEnter ()
		{
			base.OnEnter ();
			GameObject clone = (GameObject)Instantiate(gameObject.Value);
			NetworkServer.Spawn (clone);
			Finish ();
		}
	}
}
#endif