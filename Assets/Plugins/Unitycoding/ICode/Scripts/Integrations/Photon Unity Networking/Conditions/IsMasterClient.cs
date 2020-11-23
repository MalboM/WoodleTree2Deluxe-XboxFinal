#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.Photon{
	[Category("Photon")]
	[System.Serializable]
	public class IsMasterClient : Condition {
		[Tooltip("Does the result equals this condition.")]
		public FsmBool equals;

		public override bool Validate ()
		{
		
			return (PhotonNetwork.isMasterClient==equals.Value);
		}
	}
}
#endif