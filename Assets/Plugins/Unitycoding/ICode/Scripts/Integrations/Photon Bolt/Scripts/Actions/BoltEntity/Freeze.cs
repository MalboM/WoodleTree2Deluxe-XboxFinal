#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Freeze or unfreeze an entity")]
	[System.Serializable]
	public class Freeze : BoltEntityAction {
		public FsmBool pause;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			entity.Freeze (pause.Value);
			Finish ();
		}
		
	}
}
#endif