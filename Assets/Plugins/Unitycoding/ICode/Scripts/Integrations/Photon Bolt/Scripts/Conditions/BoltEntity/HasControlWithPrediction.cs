#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[Tooltip("Do we have control of this entity and are we using client side prediction")]
	[System.Serializable]
	public class HasControlWithPrediction  : BoltEntityCondition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (entity.hasControlWithPrediction==equals.Value);
		}
	}
}
#endif