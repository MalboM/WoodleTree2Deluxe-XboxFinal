using UnityEngine;
using System.Collections;

namespace ICode.Conditions.Photon{
	[Category("Photon")]
	[System.Serializable]
	public class IsEnabled : Condition {

		public override bool Validate ()
		{
			#if PUN
			return true;
			#else
			return false;
			#endif
		}
	}
}
