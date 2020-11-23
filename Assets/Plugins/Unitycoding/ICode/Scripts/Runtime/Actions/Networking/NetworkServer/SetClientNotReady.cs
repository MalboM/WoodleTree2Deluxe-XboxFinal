#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Sets the client of the connection to be not-ready.")]
	[HelpUrl("hhttp://docs.unity3d.com/ScriptReference/Networking.NetworkServer.SetClientNotReady.html")]
	[System.Serializable]
	public class SetClientNotReady : StateAction {
		public FsmInt connectionId;
		public override void OnEnter ()
		{
			base.OnEnter ();

			NetworkConnection connection = NetworkServer.connections.Where (x => x.connectionId == connectionId.Value).FirstOrDefault();
			if(connection != null){
				NetworkServer.SetClientNotReady(connection);
			}
			Finish ();
		}
	}
}
#endif