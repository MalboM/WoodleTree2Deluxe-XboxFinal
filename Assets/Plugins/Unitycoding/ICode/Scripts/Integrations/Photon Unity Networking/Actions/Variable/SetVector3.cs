#if PUN
using UnityEngine;
using System.Collections;

namespace ICode.Actions.Photon.Variables{
	[Category("Photon/Variable")]
	[Tooltip("Sets the Vector3 value of a variable.")]
	[System.Serializable]
	public class SetVector3 : StateAction {
		[SharedPersistent]
		[Tooltip("The GameObject to use.")]
		public FsmGameObject gameObject;
		[Shared]
		[Tooltip("The variable to use.")]
		public FsmVector3 variable;
		[Tooltip("The value to set.")]
		public FsmVector3 value;
		public PhotonTargets targets;

		private PhotonView photonView;

		public override void OnEnter ()
		{
			photonView = PhotonView.Get ((GameObject)gameObject.Value);
			photonView.RPC ("SetFsmVector3", targets, variable.Name,value.Value);
			Finish ();
		}

	}
}
#endif