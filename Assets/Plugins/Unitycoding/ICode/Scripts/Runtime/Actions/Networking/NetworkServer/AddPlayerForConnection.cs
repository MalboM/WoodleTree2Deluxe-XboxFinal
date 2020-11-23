#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Adds a new player for the connection.It spawns the player for all clients.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.AddPlayerForConnection.html")]
	[System.Serializable]
	public class AddPlayerForConnection : StateAction {
		[Tooltip("Connection id to set the player for.")]
		public FsmInt connectionId;
		[Tooltip("Player prefab.")]
		public FsmGameObject player;
		[Tooltip("The player controller ID number as specified by client.")]
		public FsmInt playerControllerId;
		[Tooltip("Spawn position.")]
		public FsmVector3 position;
		[Tooltip("Spawn rotation.")]
		public FsmVector3 rotation;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkConnection connection=NetworkServer.connections.Where (x => x.connectionId == connectionId.Value).FirstOrDefault();
			if(connection != null){
				GameObject thePlayer = (GameObject)Instantiate(player.Value, position.Value, Quaternion.Euler(rotation.Value));
				NetworkServer.AddPlayerForConnection(connection,thePlayer,(short)playerControllerId.Value);
			}
			Finish ();
		}
	}
}
#endif