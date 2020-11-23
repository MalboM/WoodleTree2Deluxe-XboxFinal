#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon
{
	[Category ("Photon/PhotonNetwork")]   
	[Tooltip ("Lets you either join a named room or create it on the fly - you don't have to know if someone created the room already.")]
	[System.Serializable]
	public class JoinOrCreateRoom : StateAction
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
			PhotonNetwork.JoinOrCreateRoom (roomName.Value, roomOptions, TypedLobby.Default);
			Finish ();
		}
	}
}
#endif