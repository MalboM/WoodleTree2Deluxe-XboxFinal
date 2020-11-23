#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Sets the client to be ready.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.SetClientReady.html")]
	[System.Serializable]
	public class SetClientReady : StateAction {
		public FsmInt connectionId;
		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkConnection connection = NetworkServer.connections.Where (x => x.connectionId == connectionId.Value).FirstOrDefault();
			if(connection != null){
				NetworkServer.SetClientReady(connection);
			}
			Finish ();
		}
	}
}
#endif