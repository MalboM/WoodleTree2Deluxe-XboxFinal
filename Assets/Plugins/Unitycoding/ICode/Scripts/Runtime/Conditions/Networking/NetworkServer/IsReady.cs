#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

namespace ICode.Conditions.UnityNetworking{
	[Category("UnityNetworking")]
	[Tooltip("Flag that tells if the connection has been marked as ready by a client calling NetworkClient.Ready().")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkConnection-isReady.html")]
	[System.Serializable]
	public class IsReady : Condition {
		[Tooltip("Unique identifier for this connection.")]
		public FsmInt connectionId;
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		private NetworkConnection connection;
		public override void OnEnter ()
		{
			base.OnEnter ();
			connection = NetworkServer.connections.Where (x => x.connectionId == connectionId.Value).FirstOrDefault();
		}

		public override bool Validate ()
		{
			return (connection != null && (connection.isReady == equals.Value));
		}
	}
}
#endif