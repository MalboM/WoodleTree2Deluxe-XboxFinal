#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Remove the specified player ID from the game.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.ClientScene.RemovePlayer.html")]
	[System.Serializable]
	public class RemovePlayer : StateAction {
		public FsmInt playerId;
		public override void OnEnter ()
		{
			base.OnEnter ();
			ClientScene.RemovePlayer ((short)playerId.Value);
			Finish ();
		}
	}
}
#endif