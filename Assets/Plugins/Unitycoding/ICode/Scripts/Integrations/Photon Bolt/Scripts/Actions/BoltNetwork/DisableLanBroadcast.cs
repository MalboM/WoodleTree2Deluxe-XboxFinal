#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Disable LAN broadcasting")]
	[System.Serializable]
	public class DisableLanBroadcast : StateAction {

		public override void OnEnter ()
		{
			BoltNetwork.DisableLanBroadcast ();
			Finish ();
		}
		
	}
}
#endif