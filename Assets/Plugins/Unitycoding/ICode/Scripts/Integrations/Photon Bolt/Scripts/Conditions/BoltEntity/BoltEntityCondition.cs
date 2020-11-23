#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	public abstract class BoltEntityCondition : Condition {
		[SharedPersistent]
		[Tooltip("GameObject to test.")]
		public FsmGameObject gameObject;
		
		protected BoltEntity entity;
		
		public override void OnEnter ()
		{
			entity = gameObject.Value.GetComponent<BoltEntity> ();
		}
	}
}
#endif