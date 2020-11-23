#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("The prefabId used to instantiate this entity")]
	[System.Serializable]
	public class GetPrefabId : BoltEntityAction {
		public FsmInt prefabId;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			prefabId.Value=entity.prefabId.Value;
			Finish ();
		}
		
	}
}
#endif