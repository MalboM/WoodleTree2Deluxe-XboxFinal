#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[Tooltip("Do we have control of this entity?")]
	[System.Serializable]
	public class HasControl : BoltEntityCondition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (entity.hasControl==equals.Value);
		}
	}
}
#endif