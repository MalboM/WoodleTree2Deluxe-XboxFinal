#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.Variables{
	[Category("Photon/Variable")]
	[Tooltip("Sets the bool value of a variable.")]
	[System.Serializable]
	public class SetBool : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Tooltip("The variable to use.")]
		public FsmBool variable;
		[Tooltip("The value to set.")]
		public FsmBool value;
		public PhotonTargets targets;
		private PhotonView photonView;

		public override void OnEnter ()
		{
			photonView = PhotonView.Get (gameObject.Value);
			photonView.RPC ("SetFsmBool", targets, variable.Name,value.Value);
			Finish ();

		}
	}
}
#endif