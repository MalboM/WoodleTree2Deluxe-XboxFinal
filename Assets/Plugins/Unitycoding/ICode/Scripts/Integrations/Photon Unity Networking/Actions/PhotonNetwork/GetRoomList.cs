#if PUN
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Gets the room list.")]
	[System.Serializable]
	public class GetRoomList : StateAction {
		[Shared]
		[Tooltip( "Store the rooms list.")]
		public FsmArray store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = PhotonNetwork.GetRoomList ();
			Debug.Log (PhotonNetwork.GetRoomList().Length);
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			Debug.Log (PhotonNetwork.GetRoomList().Length);
			store.Value = PhotonNetwork.GetRoomList ();
		}
	}
}
#endif