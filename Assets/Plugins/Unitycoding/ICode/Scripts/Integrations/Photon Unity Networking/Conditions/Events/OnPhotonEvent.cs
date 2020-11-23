#if PUN
using UnityEngine;
using System.Collections;
using Unitycoding;

namespace ICode.Conditions.Photon{
	[Category("Photon")]
	[System.Serializable]
	public class OnPhotonEvent : Condition {
		private bool raised;
		public PhotonNetworkingMessage type;

		public override void OnEnter ()
		{
			base.OnEnter ();
			NetworkingMessageHandler.current.RegisterListener(type.ToString(),OnRaiseEvent);
		}
		
		public override void OnExit ()
		{
			NetworkingMessageHandler.current.RemoveListener(type.ToString(),OnRaiseEvent);
			raised = false;
		}
		
		private void OnRaiseEvent(CallbackEventData eventData){
			raised = true;
		}
		
		public override bool Validate ()
		{
			if (raised) {
				raised=false;	
				return true;
			}
			return raised;
		}
	}
}
#endif