#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[Tooltip("If this entity is currently paused")]
	[System.Serializable]
	public class IsFrozen : BoltEntityCondition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (entity.isFrozen==equals.Value);
		}
	}
}
#endif