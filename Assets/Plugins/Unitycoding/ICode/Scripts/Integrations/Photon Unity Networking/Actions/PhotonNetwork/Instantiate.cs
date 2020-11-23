#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Instantiate a prefab over the network. This prefab needs to be located in the root of a Resources folder.")]
	[System.Serializable]
	public class Instantiate : StateAction {
		[Tooltip("Name of the prefab to instantiate.")]
		public FsmString prefabName;
		[NotRequired]
		[SharedPersistent]
		[Tooltip("Instantiate at targets position.")]
		public FsmGameObject target;
		[NotRequired]
		[Tooltip("Position Vector3 to apply on instantiation.")]
		public FsmVector3 position;
		[NotRequired]
		[Tooltip("Rotation euler angles to apply on instantiation.")]
		public FsmVector3 euler;
		[Tooltip("The group for this PhotonView.")]
		public FsmInt group;
		[NotRequired]
		[Shared]
		[Tooltip("Instantiated clone of the original.")]
		public FsmGameObject store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			DoInstantiate ();
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			DoInstantiate ();
		}

		private void DoInstantiate(){
			store.Value = PhotonNetwork.Instantiate (prefabName.Value, FsmUtility.GetPosition(target, position), target.Value != null && euler.IsNone?target.Value.transform.rotation:Quaternion.Euler(euler.Value), (byte)group.Value);
		}
	}
}
#endif