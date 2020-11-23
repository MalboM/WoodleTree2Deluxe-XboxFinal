#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Enable UPnP")]
	[System.Serializable]
	public class EnableUPnP : StateAction {

		public override void OnEnter ()
		{
			BoltNetwork.EnableUPnP ();
			Finish ();
		}
		
	}
}
#endif