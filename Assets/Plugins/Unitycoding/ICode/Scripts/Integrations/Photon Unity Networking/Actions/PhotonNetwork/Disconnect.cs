#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Makes this client disconnect from the photon server, a process that leaves any room and calls OnDisconnectedFromPhoton on completion.")]
	[System.Serializable]
	public class Disconnect : StateAction {

		public override void OnEnter ()
		{
			if (PhotonNetwork.connected) {
				PhotonNetwork.Disconnect();
			}
			Finish ();
		}
	}
}
#endif