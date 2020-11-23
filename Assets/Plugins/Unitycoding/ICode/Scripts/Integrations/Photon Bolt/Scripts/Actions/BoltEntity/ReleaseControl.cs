#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Releases local control of this entity")]
	[System.Serializable]
	public class ReleaseControl : BoltEntityAction {
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			entity.RevokeControl ();
			Finish ();
		}
		
	}
}
#endif