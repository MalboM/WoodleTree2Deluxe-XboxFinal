#if PUN
using UnityEngine;
using System.Collections;
using Unitycoding;

namespace ICode{
	public class NetworkingMessageHandler : CallbackHandler {
		private static NetworkingMessageHandler instance;
		public static NetworkingMessageHandler current{
			get{
				if(instance== null){
					GameObject go= new GameObject("NetworkingMessageHandler");
					instance=go.AddComponent<NetworkingMessageHandler>();
					DontDestroyOnLoad(go);
				}
				return instance;
			}
		}
		
		public override string[] Callbacks {
			get {
				return new string[]{
					"OnConnectedToPhoton",
					"OnFailedToConnectToPhoton",
					"OnDisconnectedFromPhoton",
					"OnConnectionFail",
					"OnJoinedLobby",
					"OnLeftLobby",
					"OnLeftRoom",
					"OnPhotonCreateRoomFailed",
					"OnPhotonJoinRoomFailed",
					"OnCreatedRoom",
					"OnReceivedRoomListUpdate",
					"OnJoinedRoom",
					
					"OnMasterClientSwitched",
					"OnPhotonPlayerConnected",
					"OnPhotonPlayerDisconnected",
					"OnPhotonRandomJoinFailed",
					"OnConnectedToMaster",
					"OnPhotonMaxCccuReached",
				};
			}
		}
		
		private void Awake(){
			instance = this;
		}
		
		private void OnMasterClientSwitched(PhotonPlayer newMasterClient){
			Execute ("OnMasterClientSwitched", new CallbackEventData ());
		}
		
		private void OnPhotonPlayerConnected(PhotonPlayer newPlayer){
			Execute ("OnPhotonPlayerConnected", new CallbackEventData ());
		}
		
		private void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer){
			Execute ("OnPhotonPlayerDisconnected", new CallbackEventData ());
		}
		
		private void OnPhotonRandomJoinFailed(){
			Execute ("OnPhotonRandomJoinFailed", new CallbackEventData ());
		}
		
		private void OnConnectedToMaster() {
			Execute ("OnConnectedToMaster", new CallbackEventData ());
		}
		
		private void OnPhotonMaxCccuReached(){
			Execute ("OnPhotonMaxCccuReached", new CallbackEventData ());
		}
		
		private void OnConnectedToPhoton(){
			Execute ("OnConnectedToPhoton", new CallbackEventData ());
		}
		
		private void OnFailedToConnectToPhoton(){
			Execute ("OnFailedToConnectToPhoton", new CallbackEventData ());
		}
		
		private void OnDisconnectedFromPhoton(){
			Execute ("OnDisconnectedFromPhoton", new CallbackEventData ());
		}
		
		private void OnConnectionFail(){
			Execute ("OnConnectionFail", new CallbackEventData ());
		}

		private void OnJoinedLobby(){
			Execute ("OnJoinedLobby", new CallbackEventData ());
		}
		
		private void OnLeftLobby(){
			Execute ("OnLeftLobby", new CallbackEventData ());
		}
		
		private void OnLeftRoom(){
			Execute ("OnLeftRoom", new CallbackEventData ());
		}
		
		private void OnPhotonCreateRoomFailed(){
			Execute ("OnPhotonCreateRoomFailed", new CallbackEventData ());
		}
		
		private void OnPhotonJoinRoomFailed(){
			Execute ("OnPhotonJoinRoomFailed", new CallbackEventData ());
		}
		
		private void OnCreatedRoom(){
			Execute ("OnCreatedRoom", new CallbackEventData ());
		}
		
		private void OnReceivedRoomListUpdate(){
			Execute ("OnReceivedRoomListUpdate", new CallbackEventData ());
		}
		
		private void OnJoinedRoom(){
			Execute ("OnJoinedRoom", new CallbackEventData ());
		}
	}
}
#endif
