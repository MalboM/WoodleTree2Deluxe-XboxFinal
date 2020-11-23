#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Destroys this object and corresponding objects on all clients.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.Destroy.html")]
	[System.Serializable]
	public class Destroy : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkServer.Destroy (gameObject.Value);
			Finish ();
		}
	}
}
#endif