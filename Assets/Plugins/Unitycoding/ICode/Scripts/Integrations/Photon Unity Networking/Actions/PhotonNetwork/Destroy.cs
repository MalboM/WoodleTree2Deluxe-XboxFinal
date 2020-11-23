#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon{
	[Category("Photon/PhotonNetwork")]   
	[Tooltip(" Network-Destroy the GameObject, unless it is static or not under this client's control.")]
	[System.Serializable]
	public class Destroy : StateAction {
		[SharedPersistent]
		[Tooltip( "GameObject to destroy.")]
		public FsmGameObject gameObject;

		public override void OnEnter ()
		{
			PhotonNetwork.Destroy (gameObject.Value);
			Finish ();
		}


	}
}
#endif