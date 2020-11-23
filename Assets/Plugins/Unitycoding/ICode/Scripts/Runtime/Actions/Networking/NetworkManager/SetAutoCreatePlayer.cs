#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("A flag to control whether or not player objects are automatically created on connect, and on scene change.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-autoCreatePlayer.html")]
	[System.Serializable]
	public class SetAutoCreatePlayer : NetworkManagerAction {
		[Tooltip("Value to set.")]
		public FsmBool value;

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.autoCreatePlayer = value.Value;
			Finish ();
		}
	}
}
#endif