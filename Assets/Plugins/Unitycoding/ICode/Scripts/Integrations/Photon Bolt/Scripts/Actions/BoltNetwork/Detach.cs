#if PHOTON_BOLT
using UdpKit;
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Detach a game object.")]
	[System.Serializable]
	public class Detach : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		public override void OnEnter ()
		{
			BoltNetwork.Detach (gameObject.Value);
			Finish ();
		}
		
	}
}
#endif