#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Marks all connected clients as no longer ready.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.SetAllClientsNotReady.html")]
	[System.Serializable]
	public class SetAllClientsNotReady : StateAction {
		public FsmInt connectionId;
		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkServer.SetAllClientsNotReady ();
			Finish ();
		}
	}
}
#endif