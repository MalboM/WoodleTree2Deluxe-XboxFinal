#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Conditions.PhotonBolt{
	[Category("Photon Bolt")]
	[System.Serializable]
	public class OnBoltEvent : Condition {
		private bool raised;
		public BoltNetworkingMessage type;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			BoltCallbackHandler.current.RegisterListener(type,OnRaiseEvent);
		}
		
		public override void OnExit ()
		{
			if (raised) {
				BoltCallbackHandler.current.RemoveListener(type,OnRaiseEvent);
			}
			raised = false;
		}
		
		private void OnRaiseEvent(BoltEventData eventData){
			raised = true;
		}
		
		public override bool Validate ()
		{
			return raised;
		}
	}
}
#endif