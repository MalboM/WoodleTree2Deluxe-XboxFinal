#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip("Connect to Photon as configured in the editor (saved in PhotonServerSettings file).")]
	[System.Serializable]
	public class ConnectUsingSettings : StateAction {
		[Tooltip("This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).")]
		public FsmString gameVersion;

		public override void OnEnter ()
		{
			if (!PhotonNetwork.connected) {
				PhotonNetwork.ConnectUsingSettings (gameVersion.Value);
			}
			Finish ();
		}
	}
}
#endif