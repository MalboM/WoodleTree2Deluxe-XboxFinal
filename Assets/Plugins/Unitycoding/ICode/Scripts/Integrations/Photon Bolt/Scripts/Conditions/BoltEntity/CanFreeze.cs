#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[Tooltip("Whether the entity can be paused / frozen")]
	[System.Serializable]
	public class CanFreeze : BoltEntityCondition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (entity.canFreeze==equals.Value);
		}
	}
}
#endif