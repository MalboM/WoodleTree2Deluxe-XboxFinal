#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("This causes the server to switch scenes and sets the networkSceneId.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.ServerChangeScene.html")]
	[System.Serializable]
	public class ServerChangeScene : NetworkManagerAction {
		[Tooltip("Scene name to switch to.")]
		public FsmString sceneName;


		public override void OnEnter ()
		{
			base.OnEnter ();
			manager.ServerChangeScene (sceneName.Value);
			Finish ();
		}
	}
}
#endif