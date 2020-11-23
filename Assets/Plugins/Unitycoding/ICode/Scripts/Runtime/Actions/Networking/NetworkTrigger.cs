#if !UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace ICode{
	[AddComponentMenu("Network/NetworkTrigger")]
	public class NetworkTrigger : MonoBehaviour {
		[SerializeField]
		private List<NetworkTrigger.Entry> m_Delegates;

		public List<NetworkTrigger.Entry> triggers
		{
			get
			{
				if (this.m_Delegates == null)
				{
					this.m_Delegates = new List<NetworkTrigger.Entry>();
				}
				return this.m_Delegates;
			}
			set
			{
				this.m_Delegates = value;
			}
		}

		private IEnumerator Start(){
			RegisterServerHandlers ();
			while (NetworkClient.allClients.Count < 1) {
				yield return null;
			}
			RegisterClientHandlers ();

		}

		private void Execute(NetworkTriggerType id, NetworkMessage message)
		{
			int num = 0;
			int count = this.triggers.Count;
			while (num < count)
			{
				NetworkTrigger.Entry item = this.triggers[num];
				if (item.eventID == id && item.callback != null)
				{
					item.callback.Invoke(message);
				}
				num++;
			}
		}

		[System.Serializable]
		public class Entry
		{
			public NetworkTriggerType eventID;
			
			public NetworkTrigger.TriggerEvent callback;
			
			public Entry()
			{
			}
		}

		[System.Serializable]
		public class TriggerEvent : UnityEvent<NetworkMessage>
		{
			public TriggerEvent()
			{
			}
		}

		private void RegisterClientHandlers(){
			if (NetworkClient.allClients.Count > 0) {
				NetworkClient client=NetworkClient.allClients[0];
				client.RegisterHandler (MsgType.Connect, OnClientConnect);
				client.RegisterHandler(MsgType.Disconnect,OnClientDisconnect);
				client.RegisterHandler(MsgType.Error,OnClientError);
				client.RegisterHandler(MsgType.NotReady,OnClientNotReady);
				client.RegisterHandler(MsgType.Scene,OnClientSceneChanged);
			}
		}

		private void RegisterServerHandlers(){
			NetworkServer.RegisterHandler (MsgType.Connect, OnServerConnect);
			NetworkServer.RegisterHandler (MsgType.Disconnect, OnServerDisconnect);
			NetworkServer.RegisterHandler (MsgType.Error, OnServerError);
			NetworkServer.RegisterHandler (MsgType.Ready, OnServerReady);
			NetworkServer.RegisterHandler (MsgType.AddPlayer, OnServerAddPlayer);
			NetworkServer.RegisterHandler (MsgType.RemovePlayer, OnServerRemovePlayer);
		}

		private void OnClientConnect(NetworkMessage message){

			Execute (NetworkTriggerType.OnClientConnected, message);
		}

		private void OnClientDisconnect(NetworkMessage message){
			Execute (NetworkTriggerType.OnClientDisconnect, message);
		}

		private void OnClientError(NetworkMessage message){
			Execute (NetworkTriggerType.OnClientError, message);
		}

		private void OnClientNotReady(NetworkMessage message){
			Execute (NetworkTriggerType.OnClientNotReady, message);
		}
	
		private void OnClientSceneChanged(NetworkMessage message){
			Execute (NetworkTriggerType.OnClientSceneChanged, message);
		}


		private void OnServerConnect(NetworkMessage message){
			Execute (NetworkTriggerType.OnServerConnect, message);
		}
		
		private void OnServerDisconnect(NetworkMessage message){
			Execute (NetworkTriggerType.OnServerDisconnect, message);
		}
		
		private void OnServerError(NetworkMessage message){
			Execute (NetworkTriggerType.OnServerError, message);
		}
		
		private void OnServerReady(NetworkMessage message){
			NetworkServer.SetClientReady (message.conn);
			Execute (NetworkTriggerType.OnServerReady, message);
		}

		private void OnServerAddPlayer(NetworkMessage message){
			Execute (NetworkTriggerType.OnServerAddPlayer, message);
		}

		private void OnServerRemovePlayer(NetworkMessage message){
			Execute (NetworkTriggerType.OnServerRemovePlayer, message);
		}
	}
}
#endif