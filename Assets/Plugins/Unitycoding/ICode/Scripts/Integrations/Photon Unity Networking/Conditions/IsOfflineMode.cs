#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.Photon{
	[Category("Photon")]
	[System.Serializable]
	public class IsOfflineMode : Condition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
			return (PhotonNetwork.offlineMode == equals.Value);
		}
	}
}
#endif