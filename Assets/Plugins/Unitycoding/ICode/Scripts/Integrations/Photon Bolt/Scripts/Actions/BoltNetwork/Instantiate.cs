#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Create a new game object in the simuation from a prefab")]
	[System.Serializable]
	public class Instantiate : StateAction {
	
		[Tooltip("An existing object that you want to make a copy of.")]
		public FsmGameObject original;
		[NotRequired]
		[SharedPersistent]
		[Tooltip("Optional instantiates at targets position.")]
		public FsmGameObject target;
		[NotRequired]
		[Tooltip("Position for the new object.")]
		public FsmVector3 position;
		[NotRequired]
		[Tooltip("Orientation of the new object.")]
		public FsmVector3 rotation;
		[NotRequired]
		[Shared]
		[Tooltip( "Instantiated clone of the original.")]
		public FsmGameObject store;

		public override void OnEnter ()
		{
			DoInstantiate ();
			Finish ();
		}

		private void DoInstantiate(){
			store.Value = BoltNetwork.Instantiate (original.Value, FsmUtility.GetPosition(target, position), target.Value != null && rotation.IsNone?target.Value.transform.rotation:Quaternion.Euler(rotation.Value)).gameObject;
		}

	}
}
#endif