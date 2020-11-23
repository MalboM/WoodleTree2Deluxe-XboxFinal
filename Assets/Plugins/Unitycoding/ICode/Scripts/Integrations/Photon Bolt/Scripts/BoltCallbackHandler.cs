#if PHOTON_BOLT
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace ICode{
	public class BoltCallbackHandler : Bolt.GlobalEventListener{
		private static BoltCallbackHandler instance;
		public static BoltCallbackHandler current{
			get{
				if(instance== null){
					GameObject go= new GameObject("BoltCallbackHandler");
					instance=go.AddComponent<BoltCallbackHandler>();
				}
				return instance;
			}
		}

		[HideInInspector]
		public List<BoltCallbackHandler.Entry> delegates;
		
		protected void Execute(BoltNetworkingMessage eventID,BoltEventData eventData)
		{
			if (this.delegates != null)
			{
				int num = 0;
				int count = this.delegates.Count;
				while (num < count)
				{
					BoltCallbackHandler.Entry item = this.delegates[num];
					if (item.eventID == eventID && item.callback != null)
					{
						item.callback.Invoke(eventData);
					}
					num++;
				}
			}
		}
		
		public void RegisterListener(BoltNetworkingMessage eventID,UnityAction<BoltEventData> call){
			if (delegates == null) {
				delegates= new List<BoltCallbackHandler.Entry>();		
			}
			BoltCallbackHandler.Entry entry = null;
			for (int i=0; i< delegates.Count; i++) {
				BoltCallbackHandler.Entry mEntry= delegates[i];
				if(mEntry.eventID == eventID){
					entry=mEntry;
					break;
				}
			}
			if (entry == null) {
				entry= new BoltCallbackHandler.Entry();
				entry.eventID=eventID;
				entry.callback= new BoltCallbackHandler.BoltCallbackEvent();
				delegates.Add(entry);
			}
			
			entry.callback.AddListener(call);
		}
		
		public void RemoveListener(BoltNetworkingMessage eventID,UnityAction<BoltEventData> call){
			if (delegates == null) {
				return;		
			}
			for (int i=0; i< delegates.Count; i++) {
				BoltCallbackHandler.Entry entry= delegates[i];
				if(entry.eventID == eventID){
					entry.callback.RemoveListener(call);
				}
			}
		}

		[System.Serializable]
		public class Entry
		{
			public BoltNetworkingMessage eventID;
			
			public BoltCallbackEvent callback;
			
			public Entry()
			{
				
			}
		}
		
		[System.Serializable]
		public class BoltCallbackEvent:UnityEvent<BoltEventData>{}

		public override void BoltStartBegin ()
		{
			Execute (BoltNetworkingMessage.BoltStartBegin, new BoltEventData ());
		}

		public override void BoltStartDone ()
		{
			Execute (BoltNetworkingMessage.BoltStartDone, new BoltEventData ());
		}

		public override void BoltStartFailed ()
		{
			Execute (BoltNetworkingMessage.BoltStartFailed, new BoltEventData ());
		} 

		public override void SceneLoadLocalBegin (string map)
		{
			Execute (BoltNetworkingMessage.SceneLoadLocalBegin, new BoltEventData (map));
		}

		public override void SceneLoadLocalDone (string map)
		{
			Execute (BoltNetworkingMessage.SceneLoadLocalDone, new BoltEventData (map));
		}

		public override void SceneLoadRemoteDone (BoltConnection connection)
		{
			Execute (BoltNetworkingMessage.SceneLoadRemoteDone, new BoltEventData (connection));
		}


		public override void Connected (BoltConnection connection)
		{
			Execute (BoltNetworkingMessage.Connected, new BoltEventData (connection));
		}

		public override void ConnectFailed (UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token)
		{
			Execute (BoltNetworkingMessage.ConnectFailed, new BoltEventData (endpoint,token));
		}

		public override void ConnectRequest (UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token)
		{
			Execute (BoltNetworkingMessage.ConnectRequest, new BoltEventData (endpoint,token));
		}

		public override void ConnectRefused (UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token)
		{
			Execute (BoltNetworkingMessage.ConnectRefused, new BoltEventData (endpoint,token));
		}

		public override void ConnectAttempt (UdpKit.UdpEndPoint endpoint, Bolt.IProtocolToken token)
		{
			Execute (BoltNetworkingMessage.ConnectAttempt, new BoltEventData (endpoint,token));
		}

		public override void Disconnected (BoltConnection connection)
		{
			Execute (BoltNetworkingMessage.Disconnected, new BoltEventData (connection));
		}

		public override void ControlOfEntityLost (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.ControlOfEntityLost, new BoltEventData (entity));
		}

		public override void ControlOfEntityGained (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.ControlOfEntityGained, new BoltEventData (entity));
		}

		public override void EntityAttached (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.EntityAttached, new BoltEventData (entity));
		}

		public override void EntityDetached (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.EntityDetached, new BoltEventData (entity));
		}

		public override void EntityReceived (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.EntityReceived, new BoltEventData (entity));
		}

		public override void EntityFrozen (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.EntityFrozen, new BoltEventData (entity));
		}

		public override void EntityThawed (BoltEntity entity)
		{
			Execute (BoltNetworkingMessage.EntityThawed, new BoltEventData (entity));
		}
	}
}
#endif