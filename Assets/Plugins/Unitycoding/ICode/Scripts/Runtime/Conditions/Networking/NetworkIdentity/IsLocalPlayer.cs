#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Conditions.UnityNetworking{
	[Category("UnityNetworking")]
	[Tooltip("This returns true if this object is the one that represents the player on the local machine.")]
	[System.Serializable]
	public class IsLocalPlayer : Condition {
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
			return identity.isLocalPlayer == equals.Value;
		}
	}
}
#endif