#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Conditions.UnityNetworking{
	[Category("UnityNetworking")]
	[Tooltip("This gives the current connection status of the client.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkClient-isConnected.html")]
	[System.Serializable]
	public class IsConnected : Condition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
			return (NetworkClient.allClients.Count > 0 && (NetworkClient.allClients[0].isConnected == equals.Value));
		}
	}
}
#endif