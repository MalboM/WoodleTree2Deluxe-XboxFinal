#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Closes a port to UPnP.")]
	[System.Serializable]
	public class ClosePortUPnP : StateAction {
		public FsmInt port;

		public override void OnEnter ()
		{
			BoltNetwork.ClosePortUPnP (port.Value);
			Finish ();
		}
		
	}
}
#endif