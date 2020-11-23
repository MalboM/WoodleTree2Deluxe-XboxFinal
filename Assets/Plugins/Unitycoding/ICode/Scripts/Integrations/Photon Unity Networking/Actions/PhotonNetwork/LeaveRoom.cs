#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Leave the current room.")]
	[System.Serializable]
	public class LeaveRoom : StateAction {

		public override void OnEnter ()
		{
			PhotonNetwork.LeaveRoom ();
			Finish ();
		}


	}
}
#endif