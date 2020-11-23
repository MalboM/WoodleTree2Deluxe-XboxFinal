#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Connect to a server.")]
	[System.Serializable]
	public class LoadScene : StateAction {
		public FsmString scene;

		public override void OnEnter ()
		{
			BoltNetwork.LoadScene (scene);
			Finish ();
		}
		
	}
}
#endif