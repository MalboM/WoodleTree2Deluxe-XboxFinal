#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("This is the local player ID for the player, for example like which controller a player is using. This is not the global overall player number.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.ClientScene.AddPlayer.html")]
	[System.Serializable]
	public class AddPlayer : StateAction {
		public FsmInt playerId;
		public override void OnEnter ()
		{
			base.OnEnter ();
			ClientScene.AddPlayer ((short)playerId.Value);
			Finish ();
		}
	}
}
#endif