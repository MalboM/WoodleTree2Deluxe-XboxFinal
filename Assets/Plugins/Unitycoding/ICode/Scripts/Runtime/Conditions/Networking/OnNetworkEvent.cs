#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ICode.Conditions.UnityNetworking{
	[Category("UnityNetworking")]
	[Tooltip("Internal unity network messages.")]
	[System.Serializable]
	public class OnNetworkEvent : Condition {
		public NetworkTriggerType type;
		private NetworkTrigger handler;
		private bool isTrigger;
		[Shared]
		[NotRequired]
		[Tooltip("Store the connection id.")]
		public FsmInt connectionId;

		public override void OnEnter ()
		{
			if (handler == null) {
				GameObject handlerGo=new GameObject("NetworkTrigger");
				handler = handlerGo.AddComponent<NetworkTrigger>();
			}
			NetworkTrigger.Entry entry = new NetworkTrigger.Entry ();
			entry.eventID = type;
			entry.callback = new NetworkTrigger.TriggerEvent ();
			entry.callback.AddListener (OnTrigger);
			handler.triggers.Add (entry);
			
		}
		
		public override void OnExit ()
		{
			isTrigger = false;
		}
		
		private void OnTrigger(NetworkMessage eventData){
			connectionId.Value = eventData.conn.connectionId;
			isTrigger = true;
		}
		
		public override bool Validate ()
		{
			if (isTrigger) {
				isTrigger=false;	
				return true;
			}
			return isTrigger;
		}
	}
}
#endif