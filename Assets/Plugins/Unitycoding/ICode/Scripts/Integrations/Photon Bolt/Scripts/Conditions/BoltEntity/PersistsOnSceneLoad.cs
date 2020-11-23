#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[Tooltip("Should this entity persist between scene loads")]
	[System.Serializable]
	public class PersistsOnSceneLoad  : BoltEntityCondition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (entity.persistsOnSceneLoad==equals.Value);
		}
	}
}
#endif