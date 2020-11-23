#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Sets the scope of all currently active connections for this entity. Only usable if Scope Mode has been set to Manual.")]
	[System.Serializable]
	public class SetScopeAll : BoltEntityAction {
		public FsmBool inScope;

		public override void OnEnter ()
		{
			base.OnEnter ();
			entity.SetScopeAll (inScope.Value);
			Finish ();
		}
		
	}
}
#endif