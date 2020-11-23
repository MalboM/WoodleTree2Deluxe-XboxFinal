#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Actions.UnityNetworking{
	[Category("UnityNetworking/NetworkManager")]
	[Tooltip("This finds a spawn position based on NetworkStartPosition objects in the scene.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/Networking.NetworkManager.GetStartPosition.html")]
	[System.Serializable]
	public class GetStartPosition : NetworkManagerAction {
		[Shared]
		[NotRequired]
		[Tooltip("Store the transform with NetworkStartPosition.")]
		public FsmObject transform;
		[Shared]
		[NotRequired]
		[Tooltip("Store the position of the transform with NetworkStartPosition.")]
		public FsmVector3 position;

		public override void OnEnter ()
		{
			base.OnEnter ();
			Transform mTransform = manager.GetStartPosition ();
			transform.Value = mTransform;
			if (mTransform != null) {
				position=mTransform.position;
			}
			Finish ();
		}
	}
}
#endif