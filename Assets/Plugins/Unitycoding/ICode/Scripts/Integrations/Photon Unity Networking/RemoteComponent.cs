#if PUN
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace ICode{
	[RequireComponent( typeof( PhotonView ) )]
	public class RemoteComponent : MonoBehaviour {
		[SerializeField]
		private ControlEvent isMine;
		[SerializeField]
		private ControlEvent isMasterClient;
		
		private void Start(){
			PhotonView photonView = GetComponent<PhotonView> ();
			isMine.Invoke (photonView.isMine);
			isMasterClient.Invoke (PhotonNetwork.connected?PhotonNetwork.isMasterClient:true);
		}
		
		private void OnMasterClientSwitched( PhotonPlayer newMaster )
		{
			isMasterClient.Invoke (PhotonNetwork.connected?PhotonNetwork.isMasterClient:true);
		}
		
		[System.Serializable]
		public class ControlEvent:UnityEvent<bool>{
			
		} 
	}
}
#endif