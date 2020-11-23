#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The maximum number of concurrent network connections to support.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-maxConnections.html")]
	[System.Serializable]
	public class SetMaxConnections : NetworkManagerAction {
		[Tooltip("Value to set.")]
		public FsmInt value;

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.maxConnections=value.Value;
			Finish ();
		}
	}
}
#endif