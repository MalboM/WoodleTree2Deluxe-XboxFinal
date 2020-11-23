#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[System.Serializable]
	public abstract class BoltEntityAction : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		protected BoltEntity entity;
		
		public override void OnEnter ()
		{
			entity = gameObject.Value.GetComponent<BoltEntity> ();
		}
		
	}
}
#endif