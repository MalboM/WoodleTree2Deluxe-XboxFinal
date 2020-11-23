#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[System.Serializable]
	public class SetOfflineMode : StateAction {
		[Tooltip("Value to set.")]
		public FsmBool value;

		public override void OnEnter ()
		{
			PhotonNetwork.offlineMode = value.Value;
			Finish ();
		}
	}
}
#endif