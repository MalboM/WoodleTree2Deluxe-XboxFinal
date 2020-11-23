#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon")]   
	[Tooltip("Get the photon view id.")]
	[System.Serializable]
	public class GetPhotonViewID : StateAction {
		[SharedPersistent]
		[Tooltip( "GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		public FsmInt store;
		public override void OnEnter ()
		{
			PhotonView photonView = gameObject.Value.GetComponent<PhotonView> ();
			if (photonView != null) {
				store.Value=photonView.viewID;
			}
			Finish ();
		}
		
		
	}
}
#endif