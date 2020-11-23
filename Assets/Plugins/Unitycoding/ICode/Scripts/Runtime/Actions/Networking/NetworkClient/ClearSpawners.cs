#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("This clears the registered spawn prefabs and spawn handler functions for this client.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.ClientScene.ClearSpawners.html")]
	[System.Serializable]
	public class ClearSpawners : StateAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			ClientScene.ClearSpawners ();
			Finish ();
		}
	}
}
#endif