#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Opens a port to UPnP")]
	[System.Serializable]
	public class OpenPortUPnP : StateAction {
		public FsmInt port;

		public override void OnEnter ()
		{
			BoltNetwork.OpenPortUPnP (port.Value);
			Finish ();
		}
		
	}
}
#endif