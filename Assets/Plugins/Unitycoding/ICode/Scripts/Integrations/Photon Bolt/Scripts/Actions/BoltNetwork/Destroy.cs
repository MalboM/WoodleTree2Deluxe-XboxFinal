#if PHOTON_BOLT
using UdpKit;
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Removes a game object.")]
	[System.Serializable]
	public class Destroy : StateAction {
		[SharedPersistent]
		[Tooltip("GameObject to use.")]
		public FsmGameObject gameObject;

		public override void OnEnter ()
		{
			BoltNetwork.Destroy (gameObject.Value);
			Finish ();
		}
		
	}
}
#endif