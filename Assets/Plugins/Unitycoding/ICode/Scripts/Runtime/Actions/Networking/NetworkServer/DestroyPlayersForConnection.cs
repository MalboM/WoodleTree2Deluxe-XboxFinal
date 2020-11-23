#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("This destroys all the player objects associated with a NetworkConnections on a server..")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.DestroyPlayersForConnection.html")]
	[System.Serializable]
	public class DestroyPlayersForConnection : StateAction {
		[Tooltip("Connection id.")]
		public FsmInt connectionId;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkConnection connection=NetworkServer.connections.Where (x => x.connectionId == connectionId.Value).FirstOrDefault();
			if(connection != null){
				NetworkServer.DestroyPlayersForConnection(connection);
			}
			Finish ();
		}
	}
}
#endif