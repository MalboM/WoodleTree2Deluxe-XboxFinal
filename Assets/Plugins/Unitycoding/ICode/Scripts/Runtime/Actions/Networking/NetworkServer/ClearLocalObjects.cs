#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkServer")]
	[Tooltip("This clears all of the networked objects that the server is aware of. This can be required if a scene change deleted all of the objects without destroying them in the normal manner.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkServer.ClearLocalObjects.html")]
	[System.Serializable]
	public class ClearLocalObjects : StateAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkServer.ClearLocalObjects ();
			Finish ();
		}
	}
}
#endif