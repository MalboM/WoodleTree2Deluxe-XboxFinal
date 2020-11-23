#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("The count of rooms currently in use.")]
	[System.Serializable]
	public class GetRoomCount : StateAction {
		[Shared]
		[Tooltip( "Store the count of rooms.")]
		public FsmInt store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		public override void OnEnter ()
		{
			store.Value = PhotonNetwork.countOfRooms;
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			store.Value = PhotonNetwork.countOfRooms;
		}
	}
}
#endif