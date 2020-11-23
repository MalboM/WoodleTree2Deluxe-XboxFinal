#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]  
	[Tooltip("Connect to a Photon Master Server by address, port, appID and game(client) version.")]
	[System.Serializable]
	public class ConnectToMaster : StateAction {
		[Tooltip("The server's address (either your own or Photon Cloud address).")]
		public FsmString masterServerAdress;
		[Tooltip("The server's port to connect to.")]
		public FsmInt port;
		[Tooltip("Your application ID (Photon Cloud provides you with a GUID for your game).")]
		public FsmString appID;
		[Tooltip("This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).")]
		public FsmString gameVersion;
		
		public override void OnEnter ()
		{
			if (!PhotonNetwork.connected) {
				PhotonNetwork.ConnectToMaster (masterServerAdress.Value,port.Value,appID.Value, gameVersion.Value);
			}
			Finish ();
		}
	}
}
#endif