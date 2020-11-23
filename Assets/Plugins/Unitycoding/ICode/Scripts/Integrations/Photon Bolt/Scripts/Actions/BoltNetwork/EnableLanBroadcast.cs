#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Enable LAN broadcasting")]
	[System.Serializable]
	public class EnableLanBroaddcast : StateAction {

		public override void OnEnter ()
		{
			BoltNetwork.EnableLanBroadcast ();
			Finish ();
		}
		
	}
}
#endif