#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkClient")]
	[Tooltip("Destroys all networked objects on the client.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.ClientScene.DestroyAllClientObjects.html")]
	[System.Serializable]
	public class DestroyAllClientObjects : StateAction {
		public override void OnEnter ()
		{
			base.OnEnter ();
			ClientScene.DestroyAllClientObjects ();
			Finish ();
		}
	}
}
#endif