#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.CustomRPC{
	[Category("Photon/RPC")]
	[Tooltip("Loads a level for PhotonTargets.")]
	[System.Serializable]
	public class LoadLevel : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("Level to load.")]
		public FsmString level;
		public PhotonTargets targets;

		private PhotonView photonView;
		
		public override void OnEnter ()
		{
			photonView = PhotonView.Get (gameObject.Value);
			photonView.RPC ("LoadLevel", targets, level.Value);
			Finish ();
			
		}
	}
}
#endif