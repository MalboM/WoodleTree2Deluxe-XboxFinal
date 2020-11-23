#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("Unregisters a handler for a particular message type.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.UnregisterHandler.html")]
	[System.Serializable]
	public class UnregisterHandler : StateAction {
		public FsmInt msgType;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkServer.UnregisterHandler ((short)msgType.Value);
			Finish ();
		}
	}
}
#endif