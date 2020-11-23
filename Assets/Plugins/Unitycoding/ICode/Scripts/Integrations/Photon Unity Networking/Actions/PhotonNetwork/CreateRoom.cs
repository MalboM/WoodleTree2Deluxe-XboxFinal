#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon
{
	[Category ("Photon/PhotonNetwork")]   
	[Tooltip (" Creates a room but fails if this room is existing already. Can only be called on Master Server.")]
	[System.Serializable]
	public class CreateRoom : StateAction
	{
		[Tooltip ("Unique name of the room to create.")]
		public FsmString roomName;
		[Tooltip ("Shows (or hides) this room from the lobby's listing of rooms.")]
		public FsmBool isVisible;
		[InspectorLabel ("Is Open")]
		[Tooltip ("Allows (or disallows) others to join this room.")]
		public FsmBool mIsOpen;
		[Tooltip ("Max number of players that can join the room.")]
		public FsmInt maxPlayers;

		public override void OnEnter ()
		{
			RoomOptions roomOptions = new RoomOptions ();
			roomOptions.IsVisible = isVisible.Value;
			roomOptions.IsOpen = mIsOpen.Value;
			roomOptions.MaxPlayers = (byte)maxPlayers.Value;
			PhotonNetwork.CreateRoom (roomName.Value, roomOptions, null);
			Finish ();
		}
	}
}
#endif