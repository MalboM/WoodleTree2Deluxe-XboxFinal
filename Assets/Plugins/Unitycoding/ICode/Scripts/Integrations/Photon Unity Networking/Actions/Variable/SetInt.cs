#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.Variables{
	[Category("Photon/Variable")]
	[Tooltip("Sets the int value of a variable.")]
	[System.Serializable]
	public class SetInt : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("The parameter to use.")]
		public FsmInt variable;
		[Tooltip("The value to set.")]
		public FsmInt value;
		public PhotonTargets targets;
		private PhotonView photonView;

		public override void OnEnter ()
		{
			photonView = PhotonView.Get ((GameObject)gameObject.Value);
			photonView.RPC ("SetFsmInt", targets, variable.Name,value.Value);
			Finish ();
		}
	}
}
#endif