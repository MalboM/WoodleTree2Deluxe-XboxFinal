#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Whether the local simulation can receive entities instantiated from other connections")]
	[System.Serializable]
	public class SetCanReceiveEntities : StateAction {
		public FsmBool canReceiveEntities;

		public override void OnEnter ()
		{
			BoltNetwork.SetCanReceiveEntities (canReceiveEntities.Value);
			Finish ();
		}
		
	}
}
#endif