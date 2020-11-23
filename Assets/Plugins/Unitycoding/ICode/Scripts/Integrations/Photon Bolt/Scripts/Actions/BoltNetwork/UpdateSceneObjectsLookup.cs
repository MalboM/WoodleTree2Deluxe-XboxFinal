#if PHOTON_BOLT
using UdpKit;
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[System.Serializable]
	public class UpdateSceneObjectsLookup : StateAction {

		public override void OnEnter ()
		{
			BoltNetwork.UpdateSceneObjectsLookup();
			Finish ();
		}
		
	}
}
#endif