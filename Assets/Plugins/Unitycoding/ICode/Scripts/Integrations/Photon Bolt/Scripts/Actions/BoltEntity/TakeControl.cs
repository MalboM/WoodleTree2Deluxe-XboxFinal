#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Takes local control of this entity")]
	[System.Serializable]
	public class TakeControl : BoltEntityAction {

		public override void OnEnter ()
		{
			base.OnEnter ();
			entity.TakeControl ();
			Finish ();
		}
		
	}
}
#endif