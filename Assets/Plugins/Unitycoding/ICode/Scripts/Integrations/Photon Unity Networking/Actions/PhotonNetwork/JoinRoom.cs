#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Join room by roomname.")]
	[System.Serializable]
	public class JoinRoom : StateAction {
		[Tooltip("Unique name of the room.")]
		public FsmString roomName;

		
		public override void OnEnter ()
		{
			PhotonNetwork.JoinRoom (roomName.Value);
			Finish ();
		}
	}
}
#endif