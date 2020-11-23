#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.Variables{
	[Category("Photon/Variable")]
	[Tooltip("Sets the string value of a variable.")]
	[System.Serializable]
	public class SetString : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("The variable to use.")]
		public FsmString variable;
		[Tooltip("The value to set.")]
		public FsmString value;
		public PhotonTargets targets;
		private PhotonView photonView;

		public override void OnEnter ()
		{
			photonView = PhotonView.Get ((GameObject)gameObject.Value);
			photonView.RPC ("SetFsmString", targets, variable.Name,value.Value);
			Finish ();
		}
	}
}
#endif