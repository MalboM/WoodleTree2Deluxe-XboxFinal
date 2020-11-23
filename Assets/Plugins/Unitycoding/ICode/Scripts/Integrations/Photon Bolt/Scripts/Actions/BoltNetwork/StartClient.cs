#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Start the client.")]
	[System.Serializable]
	public class StartClient : StateAction {

		public override void OnEnter ()
		{
			BoltLauncher.StartClient ();
			Finish ();
		}
		
	}
}
#endif