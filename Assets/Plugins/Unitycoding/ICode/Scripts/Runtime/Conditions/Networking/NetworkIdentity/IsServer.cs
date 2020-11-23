#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Conditions.UnityNetworking{
	[Category("UnityNetworking")]
	[Tooltip("This returns true if this object is the one that represents the player on the local machine.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkIdentity-isServer.html")]
	[System.Serializable]
	public class IsServer : Condition {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		private NetworkIdentity identity;

		public override void OnEnter ()
		{
			base.OnEnter ();
			identity = gameObject.Value.GetComponent<NetworkIdentity> ();
		}

		public override bool Validate ()
		{
			return identity.isServer == equals.Value;
		}
	}
}
#endif