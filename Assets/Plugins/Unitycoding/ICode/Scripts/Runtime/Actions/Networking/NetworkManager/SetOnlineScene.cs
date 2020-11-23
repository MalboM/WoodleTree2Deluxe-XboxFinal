#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("The scene to switch to when online.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager-onlineScene.html")]
	[System.Serializable]
	public class SetOnlineScene : NetworkManagerAction {
		[Tooltip("Value to set.")]
		public FsmString sceneName;

		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.onlineScene=sceneName.Value;
			Finish ();
		}
	}
}
#endif