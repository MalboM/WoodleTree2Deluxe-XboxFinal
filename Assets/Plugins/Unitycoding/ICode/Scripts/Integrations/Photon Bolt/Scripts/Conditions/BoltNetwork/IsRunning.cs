#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[System.Serializable]
	public class IsRunning : Condition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (BoltNetwork.isRunning==equals.Value);
		}
	}
}
#endif